using Apps.MicrosoftTeamsBot.Models.Identifiers;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.MicrosoftTeamsBot.Webhooks.Handlers.Chat;

public class MessageSentToChatWebhookHandler(InvocationContext invocationContext, [WebhookParameter(true)] ChatIdentifier chat)
    : BaseWebhookHandler(invocationContext, SubscriptionEvent, chat.ChatId)
{
    private const string SubscriptionEvent = "message";
}