using Apps.MicrosoftTeamsBot.Auth;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Graph.Models;
using Newtonsoft.Json;

namespace Apps.MicrosoftTeamsBot.DynamicHandlers;

public class ChannelHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public ChannelHandler(InvocationContext invocationContext) : base(invocationContext)
    {
    }

    public async Task<Dictionary<string, string>> GetDataAsync(
        DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var client = new MSTeamsClient(InvocationContext.AuthenticationCredentialsProviders);
        var credentials = ConnectionCredentials.FromProviders(InvocationContext.AuthenticationCredentialsProviders);
        var channels = new Dictionary<string, string>();

        if (credentials.IsApplicationConnection)
        {
            var teams = await client.Groups.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Filter = "resourceProvisioningOptions/Any(x:x eq 'Team')";
                requestConfiguration.QueryParameters.Select = new[] { "id", "displayName" };
                requestConfiguration.QueryParameters.Top = 100;
            }, cancellationToken);

            foreach (var team in teams?.Value ?? Enumerable.Empty<Group>())
            {
                if (string.IsNullOrWhiteSpace(team.Id))
                    continue;

                var teamChannels = await client.Teams[team.Id].Channels.GetAsync(cancellationToken: cancellationToken);

                foreach (var channel in teamChannels?.Value ?? Enumerable.Empty<Channel>())
                {
                    var channelName = channel.DisplayName ?? "Unnamed channel";
                    if (!MatchesSearch(context.SearchString, team.DisplayName, channelName))
                        continue;

                    var key = JsonConvert.SerializeObject(new TeamChannel { TeamId = team.Id, ChannelId = channel.Id });
                    channels[key] = $"{channelName} ({team.DisplayName} team)";
                }
            }

            return channels;
        }

        var joinedTeams = await client.Me.JoinedTeams.GetAsync(cancellationToken: cancellationToken);
        foreach (var team in joinedTeams?.Value ?? Enumerable.Empty<Team>())
        {
            var teamChannels = await client.Teams[team.Id].Channels.GetAsync(cancellationToken: cancellationToken);

            foreach (var channel in teamChannels?.Value ?? Enumerable.Empty<Channel>())
            {
                var channelName = channel.DisplayName ?? "Unnamed channel";
                if (!MatchesSearch(context.SearchString, team.DisplayName, channelName))
                    continue;

                var key = JsonConvert.SerializeObject(new TeamChannel { TeamId = team.Id, ChannelId = channel.Id });
                channels[key] = $"{channelName} ({team.DisplayName} team)";
            }
        }

        return channels;
    }

    private static bool MatchesSearch(string? search, string? teamName, string? channelName)
    {
        if (string.IsNullOrWhiteSpace(search))
            return true;

        return (teamName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false) ||
               (channelName?.Contains(search, StringComparison.OrdinalIgnoreCase) ?? false);
    }
}

public class TeamChannel
{
    public string TeamId { get; set; }

    public string ChannelId { get; set; }
}
