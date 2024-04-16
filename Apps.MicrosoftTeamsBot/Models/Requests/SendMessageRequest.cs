using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.MicrosoftTeamsBot.Models.Requests;

public class SendMessageRequest
{
    public string? Message { get; set; }

    [Display("Bot service url", Description = "Service url could be found in event output parameters")]
    public string BotServiceUrl { get; set; }

    [Display("Attachment file")]
    public FileReference? AttachmentFile { get; set; }

    //[Display("Attachment file from OneDrive")]
    //[DataSource(typeof(OneDriveFileHandler))]
    //public string? OneDriveAttachmentFileId { get; set; }
}