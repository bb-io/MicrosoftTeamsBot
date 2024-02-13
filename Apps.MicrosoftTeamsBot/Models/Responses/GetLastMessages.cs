using Apps.MicrosoftTeamsBot.Dtos;

namespace Apps.MicrosoftTeamsBot.Models.Responses
{
    public class GetLastMessages
    {
        public IEnumerable<ChatMessageDto> Messages { get; set; }
    }
}
