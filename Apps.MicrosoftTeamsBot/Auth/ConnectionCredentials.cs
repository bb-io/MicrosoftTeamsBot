using Apps.MicrosoftTeamsBot.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.MicrosoftTeamsBot.Auth;

public class ConnectionCredentials
{
    public string? ConnectionType { get; init; }
    public string? AccessToken { get; init; }
    public string? ClientId { get; init; }
    public string? TenantId { get; init; }
    public string? ClientSecret { get; init; }

    public static ConnectionCredentials FromProviders(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        var providers = authenticationCredentialsProviders.ToDictionary(x => x.KeyName, x => x.Value);

        providers.TryGetValue(CredNames.ConnectionType, out var connectionType);
        providers.TryGetValue("Authorization", out var accessToken);
        providers.TryGetValue(CredNames.ClientId, out var clientId);
        providers.TryGetValue(CredNames.TenantId, out var tenantId);
        providers.TryGetValue(CredNames.ClientSecret, out var clientSecret);

        return new ConnectionCredentials
        {
            ConnectionType = connectionType,
            AccessToken = accessToken,
            ClientId = clientId,
            TenantId = tenantId,
            ClientSecret = clientSecret
        };
    }
}
