using Apps.MicrosoftTeamsBot.Dtos;

namespace Apps.MicrosoftTeamsBot.Models.Responses;

public class DownloadFilesAttachedToMessageResponse
{
    public IEnumerable<FileDto> Files { get; set; }
}