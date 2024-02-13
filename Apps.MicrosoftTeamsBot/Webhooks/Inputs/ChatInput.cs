using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MicrosoftTeamsBot.Webhooks.Inputs;

public class ChatInput
{
    [Display("Chat")]
    [DataSource(typeof(ChatHandler))]
    public string ChatId { get; set; }
}