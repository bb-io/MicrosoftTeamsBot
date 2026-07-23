using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using System.Text.Json;

namespace Apps.MicrosoftTeamsBot.Authorization.OAuth2
{
    public class OAuth2TokenService : BaseInvocable, IOAuth2TokenService, ITokenRefreshable
    {
        private const string ExpiresAtKeyName = "expires_at";
        public OAuth2TokenService(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public bool IsRefreshToken(Dictionary<string, string> values)
            => values.TryGetValue(ExpiresAtKeyName, out var expireValue) && DateTime.UtcNow > DateTime.Parse(expireValue);

        public int? GetRefreshTokenExprireInMinutes(Dictionary<string, string> values)
        {
            if (!values.TryGetValue(ExpiresAtKeyName, out var expireValue))
                return null;

            if (!DateTime.TryParse(expireValue, out var expireDate))
                return null;

            var difference = expireDate - DateTime.UtcNow;

            return (int)difference.TotalMinutes - 5;
        }

        public async Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values, 
            CancellationToken cancellationToken) 
        { 
            const string grant_type = "refresh_token";
            var clientId = GetClientId(values);
            var clientSecret = GetClientSecret(values);
            var bodyParameters = new Dictionary<string, string>
            {
                { "grant_type", grant_type },
                { "refresh_token", values["refresh_token"] },
                { "client_id", clientId },
                { "client_secret", clientSecret }
            };
            return await RequestToken(bodyParameters, GetTokenUrl(values), cancellationToken);
        }

        public async Task<Dictionary<string, string?>> RequestToken(
            string state, 
            string code, 
            Dictionary<string, string> values, 
            CancellationToken cancellationToken)
        {
            const string grant_type = "authorization_code";
            var clientId = GetClientId(values);
            var clientSecret = GetClientSecret(values);

            var bodyParameters = new Dictionary<string, string>
            {
                { "grant_type", grant_type },
                { "client_id", clientId },
                { "client_secret", clientSecret },
                { "code", code },
                { "redirect_uri", $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode" },
            };
            return await RequestToken(bodyParameters, GetTokenUrl(values), cancellationToken);
        }

        public Task RevokeToken(Dictionary<string, string> values)
        {
            throw new NotImplementedException();
        }

        private static string GetTenant(Dictionary<string, string> values)
        {
            if (values.TryGetValue(CredNames.TenantId, out var tenantId) &&
                !string.IsNullOrWhiteSpace(tenantId))
                return tenantId;

            return "common";
        }

        private static string GetClientId(Dictionary<string, string> values)
        {
            if (values.TryGetValue(CredNames.ClientId, out var clientId) &&
                !string.IsNullOrWhiteSpace(clientId))
                return clientId;

            return ApplicationConstants.TeamsClientId;
        }

        private static string GetClientSecret(Dictionary<string, string> values)
        {
            if (values.TryGetValue(CredNames.ClientSecret, out var clientSecret) &&
                !string.IsNullOrWhiteSpace(clientSecret))
                return clientSecret;

            return ApplicationConstants.TeamsClientSecret;
        }

        private static string GetTokenUrl(Dictionary<string, string> values)
            => $"https://login.microsoftonline.com/{GetTenant(values)}/oauth2/v2.0/token";

        private async Task<Dictionary<string, string>> RequestToken(
            Dictionary<string, string> bodyParameters,
            string tokenUrl,
            CancellationToken cancellationToken)
        {
            var utcNow = DateTime.UtcNow;
            using HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            using var httpContent = new FormUrlEncodedContent(bodyParameters);
            using var response = await httpClient.PostAsync(tokenUrl, httpContent, cancellationToken);
            var responseContent = await response.Content.ReadAsStringAsync();
            var resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent)?.ToDictionary(r => r.Key, r => r.Value?.ToString())
                ?? throw new InvalidOperationException($"Invalid response content: {responseContent}");
            var expiresIn = int.Parse(resultDictionary["expires_in"]);
            var expiresAt = utcNow.AddSeconds(expiresIn);
            resultDictionary.Add(ExpiresAtKeyName, expiresAt.ToString());
            return resultDictionary;
        }
    }
}
