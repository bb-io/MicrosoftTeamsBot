using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Apps.MicrosoftTeamsBot.Webhooks.Inputs;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.MicrosoftTeamsBot.Webhooks.Handlers.Channel;

public class MessageSentToChannelWebhookHandler : BaseWebhookHandler
{
    private const string SubscriptionEvent = "message";
    
    public MessageSentToChannelWebhookHandler(InvocationContext invocationContext, [WebhookParameter(true)] ChannelInput channel) 
        : base(invocationContext, SubscriptionEvent, JsonConvert.DeserializeObject<TeamChannel>(channel.TeamChannelId).ChannelId) { }

}