using Newtonsoft.Json;

namespace Apps.MicrosoftTeamsBot.Dtos
{
    public class MessageAttachmentDto
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public Content Content { get; set; }

    }

    public class Button
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("value")]
        public string Value { get; set; }
    }

    public class Content
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("buttons")]
        public List<Button> Buttons { get; set; }
    }
}
