using System.Text.Json;
using Apps.MicrosoftTeamsBot.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;
using RestSharp;

namespace Apps.MicrosoftTeamsBot.Auth;

public static class AppTokenService
{
    private const string GraphScope = "https://graph.microsoft.com/.default";
    private const string BotFrameworkScope = "https://api.botframework.com/.default";
    private const string BotFrameworkTenant = "botframework.com";

    public static async Task<string> GetGraphAccessTokenAsync(ConnectionCredentials credentials, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(credentials.ClientId) ||
            string.IsNullOrWhiteSpace(credentials.ClientSecret) ||
            string.IsNullOrWhiteSpace(credentials.TenantId))
        {
            throw new InvalidOperationException("Application connection requires client ID, client secret, and tenant ID.");
        }

        using var httpClient = new HttpClient();
        using var httpContent = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            { "grant_type", "client_credentials" },
            { "client_id", credentials.ClientId },
            { "client_secret", credentials.ClientSecret },
            { "scope", GraphScope }
        });

        using var response = await httpClient.PostAsync(
            $"https://login.microsoftonline.com/{credentials.TenantId}/oauth2/v2.0/token",
            httpContent,
            cancellationToken);

        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);
        response.EnsureSuccessStatusCode();

        var resultDictionary = JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent)
            ?? throw new InvalidOperationException($"Invalid response content: {responseContent}");

        return resultDictionary["access_token"]?.ToString()
            ?? throw new InvalidOperationException("Access token is missing in the Graph token response.");
    }

    public static async Task<string> GetBotAccessTokenAsync(ConnectionCredentials credentials, CancellationToken cancellationToken = default)
    {
        var isOwnAppBot = string.Equals(credentials.ConnectionType, ConnectionTypes.Application, StringComparison.OrdinalIgnoreCase);
        
        if (isOwnAppBot && (string.IsNullOrWhiteSpace(credentials.ClientId) || string.IsNullOrWhiteSpace(credentials.ClientSecret)))
            throw new PluginMisconfigurationException("Application connection requires bot client ID and client secret");
        
        string clientId = (isOwnAppBot ? credentials.ClientId! : ApplicationConstants.BotClientId).Trim();
        string clientSecret = (isOwnAppBot ? credentials.ClientSecret! : ApplicationConstants.BotClientSecret).Trim();
        string scope = string.IsNullOrWhiteSpace(ApplicationConstants.BotScope) ? BotFrameworkScope : ApplicationConstants.BotScope;
        string tenantId = isOwnAppBot && !string.IsNullOrWhiteSpace(credentials.TenantId) 
            ? credentials.TenantId.Trim()
            : BotFrameworkTenant;
        
        string url = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

        var client = new RestClient();
        var request = new RestRequest(url, Method.Post)
            .AddParameter("grant_type", "client_credentials")
            .AddParameter("client_id", clientId)
            .AddParameter("client_secret", clientSecret)
            .AddParameter("scope", scope);

        var response = await client.ExecuteAsync<NotAuthResponse>(request, cancellationToken);
        if (!response.IsSuccessful || response.Data?.AccessToken is null)
            throw new PluginApplicationException($"Failed to request bot access token: {response.Content}");

        return response.Data.AccessToken;
    }
}
