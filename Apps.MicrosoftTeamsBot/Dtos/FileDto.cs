using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.MicrosoftTeamsBot.Dtos;

public class FileDto
{
    public FileDto(FileReference file)
    {
        File = file;
    }
    
    public FileReference File { get; set; }
}