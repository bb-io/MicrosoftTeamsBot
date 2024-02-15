using Apps.MicrosoftTeamsBot.Dtos;
using Apps.MicrosoftTeamsBot.Webhooks.Handlers.Chat;
using Apps.MicrosoftTeamsBot.Webhooks.Inputs;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.MicrosoftTeamsBot.Webhooks.Lists;

[WebhookList]
public class ChatWebhooks : BaseWebhookList
{
    public ChatWebhooks(InvocationContext invocationContext) : base(invocationContext) { }
    
    [Webhook("On bot mentioned in chat", typeof(MessageSentToChatWebhookHandler), 
        Description = "On bot mentioned in chat")]
    public async Task<WebhookResponse<ChannelMessageDto>> OnMessageSentToChat(WebhookRequest request, [WebhookParameter] SenderInput sender)
    {
        return await HandleWebhookRequest<ChannelMessageDto>(request);
    }
    
    //[Webhook("On message with attachments sent to chat", typeof(MessageSentToChatWebhookHandler), 
    //    Description = "This webhook is triggered when a message with attachments is sent to the chat.")]
    //public async Task<WebhookResponse<ChatMessageDto>> OnMessageWithAttachmentsSent(WebhookRequest request, 
    //    [WebhookParameter] ChatInput chat, [WebhookParameter] SenderInput sender)
    //{
    //    return await HandleWebhookRequest(request, 
    //        new ChatMessageWithAttachmentsGetter(AuthenticationCredentialsProviders, chat, sender));
    //}
    
    //[Webhook("On user mentioned in chat", typeof(MessageSentToChatWebhookHandler), 
    //    Description = "This webhook is triggered when a new message is sent to the chat with specified user mentioned.")]
    //public async Task<WebhookResponse<ChatMessageDto>> OnUserMentioned(WebhookRequest request, 
    //    [WebhookParameter] ChatInput chat, [WebhookParameter] UserInput user)
    //{
    //    return await HandleWebhookRequest(request, 
    //        new ChatMessageWithUserMentionedGetter(AuthenticationCredentialsProviders, chat, user));
    //}
}