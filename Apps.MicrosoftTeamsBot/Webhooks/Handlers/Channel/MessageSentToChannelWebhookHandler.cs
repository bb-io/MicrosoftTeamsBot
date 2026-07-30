using Apps.MicrosoftTeamsBot.Models.Identifiers;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.MicrosoftTeamsBot.Webhooks.Handlers.Channel;

public class MessageSentToChannelWebhookHandler(InvocationContext invocationContext, [WebhookParameter(true)] ChannelIdentifier channel)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent, channel.Resolve().ChannelId)
{
    private const string SubscriptionEvent = "message";
}