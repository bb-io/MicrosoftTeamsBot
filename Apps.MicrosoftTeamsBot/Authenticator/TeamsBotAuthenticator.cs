using Apps.MicrosoftTeamsBot.Constants;
using Apps.MicrosoftTeamsBot.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;

namespace Apps.MicrosoftTeamsBot.Authenticator;

public class TeamsBotAuthenticator : IAuthenticator
{
    private static string? _token;
    private static DateTime _expiresAt = DateTime.MinValue;
    private static readonly RestClient TokenClient = new();
    
    public async ValueTask Authenticate(IRestClient client, RestRequest request)
    {
        if (!string.IsNullOrWhiteSpace(_token) && DateTime.UtcNow < _expiresAt)
        { 
            request.AddHeader("Authorization", $"Bearer {_token}");
            return;
        }
        
        string endpoint = "https://login.microsoftonline.com/botframework.com/oauth2/v2.0/token";
        var authBotRequest = new RestRequest(endpoint, Method.Post)
            .AddParameter("grant_type", "client_credentials")
            .AddParameter("client_id", ApplicationConstants.BotClientId)
            .AddParameter("client_secret", ApplicationConstants.BotClientSecret)
            .AddParameter("scope", "https://api.botframework.com/.default");

        var response = await TokenClient.ExecuteAsync(authBotRequest);
        if (!response.IsSuccessful || string.IsNullOrWhiteSpace(response.Content))
            throw new PluginApplicationException($"Bot token request failed. Response: {response.Content}");

        var deserialized = JsonConvert.DeserializeObject<NotAuthResponse>(response.Content);
        if (deserialized is null || string.IsNullOrWhiteSpace(deserialized.AccessToken))
            throw new PluginApplicationException("Deserialized auth bot response was empty");

        _token = deserialized.AccessToken;
        _expiresAt = DateTime.UtcNow.AddSeconds(Math.Max(deserialized.ExpiresIn - 300, 60));
        request.AddHeader("Authorization", $"Bearer {_token}");
    }
}