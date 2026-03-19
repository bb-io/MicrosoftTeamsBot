using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.AspNetCore.WebUtilities;

namespace Apps.MicrosoftTeamsBot.Authorization.OAuth2
{
    public class OAuth2AuthorizeService : BaseInvocable, IOAuth2AuthorizeService
    {
        public OAuth2AuthorizeService(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        public string GetAuthorizationUrl(Dictionary<string, string> values)
        {
            string bridgeOauthUrl = $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/oauth";

            var tenantId = GetTenant(values);
            var oauthUrl = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/authorize";

            var adminPermissionRequired = values.First(v => v.Key == CredNames.AdminPermissionRequired).Value.ToLower();
            var requiredScope = adminPermissionRequired == "yes"
                ? ApplicationConstants.TeamsFullScope
                : ApplicationConstants.TeamsLimitedScope;

            var clientId = GetClientId(values);
            
            var parameters = new Dictionary<string, string>
            {
                { "client_id", clientId },
                { "redirect_uri", $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode" },
                { "scope", requiredScope },
                { "state", values["state"] },
                { "response_type", "code" },
                { "authorization_url", oauthUrl},
                { "actual_redirect_uri", InvocationContext.UriInfo.AuthorizationCodeRedirectUri.ToString() },
            };
            return QueryHelpers.AddQueryString(bridgeOauthUrl, parameters);
        }

        private static string GetTenant(Dictionary<string, string> values)
        {
            if (values.TryGetValue(global::Apps.MicrosoftTeamsBot.CredNames.TenantId, out var tenantId) &&
                !string.IsNullOrWhiteSpace(tenantId))
                return tenantId;

            return "common";
        }

        private static string GetClientId(Dictionary<string, string> values)
        {
            if (values.TryGetValue(global::Apps.MicrosoftTeamsBot.CredNames.ClientId, out var clientId) &&
                !string.IsNullOrWhiteSpace(clientId))
                return clientId;

            return ApplicationConstants.TeamsClientId;
        }
    }
}
