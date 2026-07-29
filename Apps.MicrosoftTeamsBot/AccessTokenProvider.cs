using Apps.MicrosoftTeamsBot.Auth;
using Blackbird.Applications.Sdk.Common.Exceptions;
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

    public Task<string> GetAuthorizationTokenAsync(
        Uri uri,
        Dictionary<string, object>? additionalAuthenticationContext = null,
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(_credentials.AccessToken))
            throw new PluginMisconfigurationException("The connection has no access token. Please reconnect");

        return Task.FromResult(_credentials.AccessToken);
    }
}
