using Apps.MicrosoftTeamsBot.Constants;
using Apps.MicrosoftTeamsBot.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.MicrosoftTeamsBot.Auth;

public static class AppTokenService
{
    private const string BotFrameworkScope = "https://api.botframework.com/.default";
    private const string BotFrameworkTenant = "botframework.com";

    public static async Task<string> GetBotAccessTokenAsync(CancellationToken cancellationToken = default)
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
