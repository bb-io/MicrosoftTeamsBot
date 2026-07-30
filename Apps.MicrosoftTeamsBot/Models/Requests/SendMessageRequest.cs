using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.MicrosoftTeamsBot.Models.Requests;

public class SendMessageRequest
{
    public string? Message { get; set; }

    [Display("Attachment file")]
    public FileReference? AttachmentFile { get; set; }
}