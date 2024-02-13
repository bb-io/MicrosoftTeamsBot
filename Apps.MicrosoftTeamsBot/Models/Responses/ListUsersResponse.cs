using Apps.MicrosoftTeamsBot.Dtos;

namespace Apps.MicrosoftTeamsBot.Models.Responses
{
    public class ListUsersResponse
    {
        public IEnumerable<UserDto> Users { get; set; }
    }
}
