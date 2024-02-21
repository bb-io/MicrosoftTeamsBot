using Apps.MicrosoftTeamsBot.Dtos;
using Apps.MicrosoftTeamsBot.Webhooks.Handlers.Channel;
using Apps.MicrosoftTeamsBot.Webhooks.Inputs;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.MicrosoftTeamsBot.Webhooks.Lists;

[WebhookList]
public class ChannelWebhooks : BaseWebhookList
{
    public ChannelWebhooks(InvocationContext invocationContext) : base(invocationContext) { }
    
    [Webhook("On bot mentioned in channel", typeof(MessageSentToChannelWebhookHandler), 
        Description = "On bot mentioned in channel")]
    public async Task<WebhookResponse<ChannelMessageDto>> OnMessageSent(WebhookRequest request,
        [WebhookParameter] SenderInput sender)
    {
        return await HandleWebhookRequest<ChannelMessageDto>(request);
    }
    
    //[Webhook("On message with attachments sent to channel", typeof(MessageSentToChannelWebhookHandler), 
    //    Description = "This webhook is triggered when a message with attachments is sent to the channel.")]
    //public async Task<WebhookResponse<ChannelMessageDto>> OnMessageWithAttachmentSent(WebhookRequest request,
    //    [WebhookParameter] SenderInput sender)
    //{
    //    return await HandleWebhookRequest(request, 
    //        new ChannelMessageWithAttachmentsGetter(AuthenticationCredentialsProviders, sender));
    //}
    
    //[Webhook("On user mentioned in channel", typeof(MessageSentToChannelWebhookHandler), 
    //    Description = "This webhook is triggered when a new message is sent to the channel with specified user mentioned.")]
    //public async Task<WebhookResponse<ChannelMessageDto>> OnUserMentioned(WebhookRequest request, 
    //    [WebhookParameter] UserInput user)
    //{
    //    return await HandleWebhookRequest(request, 
    //        new ChannelMessageWithUserMentionedGetter(AuthenticationCredentialsProviders, user));
    //}
}