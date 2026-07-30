using Apps.MicrosoftTeamsBot.Dtos;
using Apps.MicrosoftTeamsBot.Models.Identifiers;
using Apps.MicrosoftTeamsBot.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.MicrosoftTeamsBot.Actions;

[ActionList("Chats")]
public class ChatActions(InvocationContext invocationContext) : MsTeamsBotInvocable(invocationContext)
{
    [Action("Reply to message in chat", Description = "Post a reply to an existing message in a chat")]
    public async Task<ChatMessageDto> ReplyToMessageInChat(
        [ActionParameter] ChatIdentifier chatIdentifier, 
        [ActionParameter] MessageIdentifier messageIdentifier, 
        [ActionParameter] SendMessageRequest input)
    {
        string endpoint =
            $"v3/conversations/{chatIdentifier.ChatId};messageid={messageIdentifier.MessageId}/activities/{messageIdentifier.MessageId}";
        
        var botClient = new MSTeamsBotClient(input.BotServiceUrl);
        var botRequest = new RestRequest(endpoint, Method.Post)
            .AddJsonBody(new 
            {
                type = "message",
                text = input.Message
            });
        return await botClient.ExecuteWithErrorHandling<ChatMessageDto>(botRequest);
    }

    [Action("Send message to chat", Description = "Post a new message to the specified chat")]
    public async Task<ChatMessageDto> SendMessageToChat(
        [ActionParameter] ChatIdentifier chatIdentifier, 
        [ActionParameter] SendMessageRequest input)
    {
        var botClient = new MSTeamsBotClient(input.BotServiceUrl);
        var botRequest = new RestRequest($"v3/conversations/{chatIdentifier.ChatId}/activities", Method.Post)
            .AddJsonBody(new
            {
                type = "message",
                text = input.Message
            });
        return await botClient.ExecuteWithErrorHandling<ChatMessageDto>(botRequest);
    }
}