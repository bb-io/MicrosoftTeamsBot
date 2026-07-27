using Apps.MicrosoftTeamsBot.Constants;
using Apps.MicrosoftTeamsBot.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using JsonSerializer = System.Text.Json.JsonSerializer;

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
        string url = $"https://login.microsoftonline.com/{BotFrameworkTenant}/oauth2/v2.0/token";

        var client = new RestClient();
        var request = new RestRequest(url, Method.Post)
            .AddParameter("grant_type", "client_credentials")
            .AddParameter("client_id", ApplicationConstants.BotClientId)
            .AddParameter("client_secret", ApplicationConstants.BotClientSecret)
            .AddParameter("scope", BotFrameworkScope);

        var response = await client.ExecuteAsync(request, cancellationToken);
        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
            throw new PluginApplicationException($"Bot token request failed. Response: {response.Content}");

        var deserialized = JsonConvert.DeserializeObject<NotAuthResponse>(response.Content);
        return deserialized is null 
            ? throw new PluginApplicationException("Deserialized auth bot response was empty") 
            : deserialized.AccessToken;
    }
}
