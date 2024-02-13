using Blackbird.Applications.Sdk.Common;

namespace Apps.MicrosoftTeamsBot.Models.Identifiers;

public class MessageIdentifier
{
    [Display("Message ID")]
    public string MessageId { get; set; }
}