using Newtonsoft.Json;

namespace Apps.MicrosoftTeamsBot.Dtos
{
    public class ChannelMessageSendDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("attachments")]
        public List<MessageAttachmentDto> Attachments { get; set; }
    }
}
