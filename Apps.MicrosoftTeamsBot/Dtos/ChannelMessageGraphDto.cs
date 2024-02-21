using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Blackbird.Applications.Sdk.Common;
using Microsoft.Graph.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.MicrosoftTeamsBot.Dtos
{

    public class ChannelMessageGraphDto
    {
        public ChannelMessageGraphDto(ChatMessage message)
        {
            Id = message.Id;
            Content = message.Body.Content;
            From = message.From.User.DisplayName;
            TeamChannelId = JsonConvert.SerializeObject(new TeamChannel
            { TeamId = message.ChannelIdentity.TeamId, ChannelId = message.ChannelIdentity.ChannelId });
        }

        [Display("Message ID")]
        public string Id { get; set; }

        public string Content { get; set; }

        public string From { get; set; }

        [Display("Channel")]
        public string TeamChannelId { get; set; }
    }
}
