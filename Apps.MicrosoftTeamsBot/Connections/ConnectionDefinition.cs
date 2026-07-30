using Apps.MicrosoftTeamsBot.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.MicrosoftTeamsBot.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = ConnectionTypes.OAuth,
            DisplayName = "OAuth2",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties =
            [
                new(CredNames.AdminPermissionRequired)
                {
                    DisplayName = "Channel messages scope required",
                    DataItems =
                    [
                        new("yes", "Yes"),
                        new("no", "No")
                    ]
                }
            ]
        },
        new()
        {
            Name = ConnectionTypes.OAuthCustomApp,
            DisplayName = "OAuth2 (Client app)",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties =
            [
                new(CredNames.AdminPermissionRequired)
                {
                    DisplayName = "Channel messages scope required",
                    DataItems =
                    [
                        new("yes", "Yes"),
                        new("no", "No")
                    ]
                },
                new(CredNames.ClientId) { DisplayName = "Application (client) ID" },
                new(CredNames.TenantId) { DisplayName = "Directory (tenant) ID" },
                new(CredNames.ClientSecret) { DisplayName = "Client secret", Sensitive = true }
            ]
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
    {
        var providers = values
            .Select(x => new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                x.Key,
                x.Value))
            .ToList();

        var connectionType = values[nameof(ConnectionPropertyGroup)] switch
        {
            var ct when ConnectionTypes.SupportedConnectionTypes.Contains(ct) => ct,
            _ => throw new Exception($"Unknown connection type: {values[nameof(ConnectionPropertyGroup)]}")
        };

        providers.Add(new AuthenticationCredentialsProvider(
            AuthenticationCredentialsRequestLocation.None,
            CredNames.ConnectionType,
            connectionType));

        if (values.TryGetValue("access_token", out var accessToken))
        {
            providers.Add(new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                "Authorization",
                accessToken));
        }

        return providers;
    }
}
