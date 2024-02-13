using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Blackbird.Applications.Sdk.Common;
using Microsoft.Graph.Models;
using Newtonsoft.Json;

namespace Apps.MicrosoftTeamsBot.Dtos;

//public ChannelMessageDto(ChatMessage message)
//{
//    Id = message.Id;
//    Content = message.Body.Content;
//    From = message.From.User.DisplayName;
//    TeamChannelId = JsonConvert.SerializeObject(new TeamChannel 
//        { TeamId = message.ChannelIdentity.TeamId, ChannelId = message.ChannelIdentity.ChannelId });
//}

//[Display("Message ID")]
//public string Id { get; set; }

//public string Content { get; set; }

//public string From { get; set; }

//[Display("Channel")]
//public string TeamChannelId { get; set; }


    public class ChannelMessageDto
    {
        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("textFormat")]
        public string TextFormat { get; set; }

        [JsonProperty("attachments")]
        public List<Attachment> Attachments { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("timestamp")]
        public DateTime Timestamp { get; set; }

        [JsonProperty("localTimestamp")]
        public DateTime LocalTimestamp { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("serviceUrl")]
        public string ServiceUrl { get; set; }

        [JsonProperty("from")]
        public From From { get; set; }

        [JsonProperty("conversation")]
        public Conversation Conversation { get; set; }

        [JsonProperty("recipient")]
        public Recipient Recipient { get; set; }

        [JsonProperty("entities")]
        public List<Entity> Entities { get; set; }

        [JsonProperty("channelData")]
        public ChannelData ChannelData { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("localTimezone")]
        public string LocalTimezone { get; set; }
    }

    public class Attachment
    {
        [JsonProperty("contentType")]
        public string ContentType { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class Channel
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class ChannelData
    {
        [JsonProperty("teamsChannelId")]
        public string TeamsChannelId { get; set; }

        [JsonProperty("teamsTeamId")]
        public string TeamsTeamId { get; set; }

        [JsonProperty("channel")]
        public Channel Channel { get; set; }

        [JsonProperty("team")]
        public Team Team { get; set; }

        [JsonProperty("tenant")]
        public Tenant Tenant { get; set; }
    }

    public class Conversation
    {
        [JsonProperty("isGroup")]
        public bool IsGroup { get; set; }

        [JsonProperty("conversationType")]
        public string ConversationType { get; set; }

        [JsonProperty("tenantId")]
        public string TenantId { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Entity
    {
        [JsonProperty("mentioned")]
        public Mentioned Mentioned { get; set; }

        [JsonProperty("text")]
        public string Text { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("locale")]
        public string Locale { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("platform")]
        public string Platform { get; set; }

        [JsonProperty("timezone")]
        public string Timezone { get; set; }
    }

    public class From
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("aadObjectId")]
        public string AadObjectId { get; set; }
    }

    public class Mentioned
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Recipient
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class Team
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class Tenant
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }