﻿using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;

namespace Apps.MicrosoftTeamsBot.DynamicHandlers;

public class ChannelHandler : BaseInvocable, IAsyncDataSourceHandler
{
    public ChannelHandler(InvocationContext invocationContext) : base(invocationContext)
    {    
    }

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, 
        CancellationToken cancellationToken)
    {
        var client = new MSTeamsClient(InvocationContext.AuthenticationCredentialsProviders);
        var joinedTeams = await client.Me.JoinedTeams.GetAsync(cancellationToken: cancellationToken);
        var channels = new Dictionary<string, string>();

        foreach (var team in joinedTeams.Value)
        {
            var teamChannels = await client.Teams[team.Id].Channels.GetAsync(cancellationToken: cancellationToken);

            foreach (var channel in teamChannels.Value)
            {
                var key = channel.Id;
                channels[key] = $"{channel.DisplayName} ({team.DisplayName} team)";
            }
        }

        return channels;
    }
}

public class TeamChannel
{
    public string TeamId { get; set; }
    public string ChannelId { get; set; }
}