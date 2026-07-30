using Apps.MicrosoftTeamsBot.Dtos;
using Apps.MicrosoftTeamsBot.Webhooks.Handlers.Chat;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.MicrosoftTeamsBot.Webhooks.Lists;

[WebhookList("Chats")]
public class ChatWebhooks(InvocationContext invocationContext) : BaseWebhookList(invocationContext)
{
    [Webhook("On bot mentioned in chat", typeof(MessageSentToChatWebhookHandler), Description = "On bot mentioned in chat")]
    public async Task<WebhookResponse<ChannelMessageDto>> OnMessageSentToChat(WebhookRequest request)
    {
        return await HandleWebhookRequest<ChannelMessageDto>(request);
    }
}