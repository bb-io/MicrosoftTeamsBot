using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MicrosoftTeamsBot.Webhooks.Inputs;

public class ChannelInput
{
    [Display("Channel")]
    [DataSource(typeof(ChannelHandler))]
    public string TeamChannelId { get; set; } // should be deserialized into TeamChannel class
}