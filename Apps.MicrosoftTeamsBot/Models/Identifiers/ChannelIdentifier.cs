using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MicrosoftTeamsBot.Models.Identifiers;

public class ChannelIdentifier
{
    [Display("Channel")]
    [DataSource(typeof(ChannelHandler))]
    public string TeamChannelId { get; set; } // should be deserialized into TeamChannel class
}