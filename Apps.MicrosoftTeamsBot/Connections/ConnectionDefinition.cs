using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.MicrosoftTeamsBot.Connections
{
    public class ConnectionDefinition : IConnectionDefinition
    {

        public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>()
        {
            new()
            {
                Name = global::Apps.MicrosoftTeamsBot.ConnectionTypes.OAuth,
                DisplayName = "OAuth2",
                AuthenticationType = ConnectionAuthenticationType.OAuth2,
                ConnectionUsage = ConnectionUsage.Actions,
                ConnectionProperties = new List<ConnectionProperty>
                {
                    new(CredNames.AdminPermissionRequired)
                    {
                        DisplayName = "Channel messages scope required",
                        DataItems =
                        [
                            new("yes", "Yes"),
                            new("no", "No")
                        ]
                    }
                }
            },
            new()
            {
                Name = ConnectionTypes.OAuthCustomApp,
                DisplayName = "OAuth2 (Client app)",
                AuthenticationType = ConnectionAuthenticationType.OAuth2,
                ConnectionUsage = ConnectionUsage.Actions,
                ConnectionProperties = new List<ConnectionProperty>
                {
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
                }
            }
        };

        public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
            Dictionary<string, string> values)
        {
            var token = values.First(v => v.Key == "access_token");
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                "Authorization",
                $"{token.Value}"
            );
        }
    }
}
