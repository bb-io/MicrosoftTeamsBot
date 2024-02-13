using Apps.MicrosoftTeamsBot.Webhooks.Inputs;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.MicrosoftTeamsBot.Webhooks.Handlers.Chat;

public class MessageSentToChatWebhookHandler : BaseWebhookHandler
{
    private const string SubscriptionEvent = "message";
    
    public MessageSentToChatWebhookHandler(InvocationContext invocationContext, [WebhookParameter(true)] ChatInput chat) : 
        base(invocationContext, SubscriptionEvent, chat.ChatId) { }

}