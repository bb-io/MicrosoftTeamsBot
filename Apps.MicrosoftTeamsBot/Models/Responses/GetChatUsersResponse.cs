using Apps.MicrosoftTeamsBot.Dtos;

namespace Apps.MicrosoftTeamsBot.Models.Responses
{
    public class GetChatUsersResponse
    {
        public IEnumerable<ChatMemberDto> Members { get; set; }
    }
}
