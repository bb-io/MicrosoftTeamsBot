using Apps.MicrosoftTeamsBot.Auth;
using Microsoft.Kiota.Abstractions.Authentication;

namespace Apps.MicrosoftTeamsBot;

public class AccessTokenProvider : IAccessTokenProvider
{
    private readonly ConnectionCredentials _credentials;

    public AccessTokenProvider(ConnectionCredentials credentials)
    {
        _credentials = credentials;
    }

    public AllowedHostsValidator AllowedHostsValidator { get; } = new(["graph.microsoft.com"]);

    public async Task<string> GetAuthorizationTokenAsync(
        Uri uri,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        if (!string.IsNullOrWhiteSpace(_credentials.AccessToken))
            return _credentials.AccessToken;

        return await AppTokenService.GetGraphAccessTokenAsync(_credentials, cancellationToken);
    }
}
