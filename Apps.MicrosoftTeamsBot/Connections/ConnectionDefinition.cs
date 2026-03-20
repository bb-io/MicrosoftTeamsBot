using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.MicrosoftTeamsBot.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>()
    {
        new()
        {
            Name = ConnectionTypes.OAuth,
            DisplayName = "OAuth2",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionUsage = ConnectionUsage.Actions,
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
            ConnectionUsage = ConnectionUsage.Actions,
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
        },
        new()
        {
            Name = ConnectionTypes.Application,
            DisplayName = "Application",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
            ConnectionUsage = ConnectionUsage.Actions,
            ConnectionProperties =
            [
                new(CredNames.ClientId) { DisplayName = "Application (client) ID" },
                new(CredNames.TenantId) { DisplayName = "Directory (tenant) ID" },
                new(CredNames.ClientSecret) { DisplayName = "Client secret", Sensitive = true }
            ]
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
    {
        if (values.TryGetValue("access_token", out var token))
        {
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                "Authorization",
                token);
        }

        if (values.TryGetValue(CredNames.ClientId, out var clientId))
        {
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                CredNames.ClientId,
                clientId);
        }

        if (values.TryGetValue(CredNames.TenantId, out var tenantId))
        {
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                CredNames.TenantId,
                tenantId);
        }

        if (values.TryGetValue(CredNames.ClientSecret, out var clientSecret))
        {
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                CredNames.ClientSecret,
                clientSecret);
        }

        if (values.ContainsKey(CredNames.ClientId) &&
            values.ContainsKey(CredNames.ClientSecret) &&
            values.ContainsKey(CredNames.TenantId) &&
            !values.ContainsKey("access_token"))
        {
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                CredNames.ConnectionType,
                ConnectionTypes.Application);
        }
    }
}
