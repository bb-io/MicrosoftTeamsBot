using Apps.MicrosoftTeamsBot.Auth;
using Blackbird.Applications.Sdk.Common.Authentication;
using Microsoft.Graph;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Apps.MicrosoftTeamsBot;

public class MSTeamsClient : GraphServiceClient
{
    public MSTeamsClient(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
        : base(GetAuthenticationProvider(authenticationCredentialsProviders))
    {
    }

    private static BaseBearerTokenAuthenticationProvider GetAuthenticationProvider(
        IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var credentials = ConnectionCredentials.FromProviders(authenticationCredentialsProviders);
        var accessTokenProvider = new AccessTokenProvider(credentials);
        return new BaseBearerTokenAuthenticationProvider(accessTokenProvider);
    }
}
