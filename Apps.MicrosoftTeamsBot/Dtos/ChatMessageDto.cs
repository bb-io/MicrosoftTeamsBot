using Blackbird.Applications.Sdk.Common;
using Microsoft.Graph.Models;

namespace Apps.MicrosoftTeamsBot.Dtos
{
    public class ChatMessageDto
    {
        
        [Display("Message ID")]
        public string Id { get; set; }

    }
}
