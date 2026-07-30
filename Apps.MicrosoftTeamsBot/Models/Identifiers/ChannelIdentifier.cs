using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;

namespace Apps.MicrosoftTeamsBot.Models.Identifiers;

public class ChannelIdentifier
{
    [Display("Channel"), DataSource(typeof(ChannelHandler))]
    public string TeamChannelId { get; set; } = string.Empty;

    public TeamChannel Resolve()
    {
        TeamChannel? teamChannel = null;
        
        try
        {
            teamChannel = JsonConvert.DeserializeObject<TeamChannel>(TeamChannelId);
        }
        catch (JsonException)
        {
            // Handled below
        }
        
        if (teamChannel is null || string.IsNullOrWhiteSpace(teamChannel.ChannelId) || string.IsNullOrWhiteSpace(teamChannel.TeamId))
            throw new PluginMisconfigurationException("Please select a valid Channel value from the dropdown");

        return teamChannel;
    }
}