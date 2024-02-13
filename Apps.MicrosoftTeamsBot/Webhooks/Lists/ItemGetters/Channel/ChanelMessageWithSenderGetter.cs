using Apps.MicrosoftTeamsBot.Dtos;
using Apps.MicrosoftTeamsBot.Webhooks.Inputs;
using Apps.MicrosoftTeamsBot.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.MicrosoftTeamsBot.Webhooks.Lists.ItemGetters.Channel;

public class ChannelMessageWithSenderGetter : ItemGetter<ChannelMessageDto>
{
    private readonly SenderInput _sender;

    public ChannelMessageWithSenderGetter(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        SenderInput sender)
        : base(authenticationCredentialsProviders)
    {
        _sender = sender;
    }

    public override async Task<ChannelMessageDto?> GetItem(EventPayload eventPayload)
    {
        var client = new MSTeamsClient(AuthenticationCredentialsProviders);
        var teamId = GetIdFromEndpoint(eventPayload.ResourceData.Endpoint, "teams");
        var channelId = GetIdFromEndpoint(eventPayload.ResourceData.Endpoint, "channels");
        var message = await client.Teams[teamId].Channels[channelId].Messages[eventPayload.ResourceData.Id].GetAsync();

        if (_sender.UserId is not null && _sender.UserId != message.From.User.Id)
            return null;
        
        return new ChannelMessageDto(message);
    }
}