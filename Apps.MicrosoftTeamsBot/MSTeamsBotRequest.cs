using Apps.MicrosoftTeamsBot.Auth;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using RestSharp;

namespace Apps.MicrosoftTeamsBot;

public class MSTeamsBotRequest : BlackBirdRestRequest
{
    public MSTeamsBotRequest(string resource, Method method, IEnumerable<AuthenticationCredentialsProvider> creds)
        : base(resource, method, creds)
    {
    }

    protected override void AddAuth(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var credentials = ConnectionCredentials.FromProviders(creds);
        var accessToken = AppTokenService.GetBotAccessTokenAsync(credentials).GetAwaiter().GetResult();
        this.AddHeader("Authorization", $"Bearer {accessToken}");
    }
}
