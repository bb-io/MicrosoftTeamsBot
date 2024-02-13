using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MicrosoftTeamsBot.Models.Identifiers;

public class ChatIdentifier
{
    [Display("Chat")]
    [DataSource(typeof(ChatHandler))]
    public string ChatId { get; set; }
}