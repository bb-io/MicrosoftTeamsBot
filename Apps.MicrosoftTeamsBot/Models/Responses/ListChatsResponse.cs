using Apps.MicrosoftTeamsBot.Dtos;

namespace Apps.MicrosoftTeamsBot.Models.Responses
{
    public class ListChatsResponse
    {
        public IEnumerable<ChatDto> Chats { get; set; }
    }
}
