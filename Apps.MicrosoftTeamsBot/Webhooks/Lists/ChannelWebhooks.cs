using Apps.MicrosoftTeamsBot.Dtos;
using Apps.MicrosoftTeamsBot.Webhooks.Handlers.Channel;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;

namespace Apps.MicrosoftTeamsBot.Webhooks.Lists;

[WebhookList("Channels")]
public class ChannelWebhooks(InvocationContext invocationContext) : BaseWebhookList(invocationContext)
{
    [Webhook("On bot mentioned in channel", typeof(MessageSentToChannelWebhookHandler), Description = "On bot mentioned in channel")]
    public async Task<WebhookResponse<ChannelMessageDto>> OnMessageSent(WebhookRequest request)
    {
        return await HandleWebhookRequest<ChannelMessageDto>(request);
    }
}