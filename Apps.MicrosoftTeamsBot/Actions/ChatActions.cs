using Apps.MicrosoftTeamsBot.Dtos;
using Apps.MicrosoftTeamsBot.Models.Identifiers;
using Apps.MicrosoftTeamsBot.Models.Requests;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using RestSharp;

namespace Apps.MicrosoftTeamsBot.Actions
{
    [ActionList("Chats")]
    public class ChatActions : BaseInvocable
    {
        private readonly IEnumerable<AuthenticationCredentialsProvider> _authenticationCredentialsProviders;

        private readonly IFileManagementClient _fileManagementClient;

        public ChatActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient) : base(invocationContext)
        {
            _authenticationCredentialsProviders = invocationContext.AuthenticationCredentialsProviders;
            _fileManagementClient = fileManagementClient;
        }

        [Action("Reply to message in chat", Description = "Reply to message in chat")]
        public async Task<ChatMessageDto> ReplyToMessageInChat([ActionParameter] ChatIdentifier chatIdentifier,
        [ActionParameter] MessageIdentifier messageIdentifier, [ActionParameter] SendMessageRequest input)
        {
            var botClient = new MSTeamsBotClient(input.BotServiceUrl);
            var botRequest = new MSTeamsBotRequest(
                $"v3/conversations/{chatIdentifier.ChatId};messageid={messageIdentifier.MessageId}/activities/{messageIdentifier.MessageId}",
                Method.Post, _authenticationCredentialsProviders);
            botRequest.AddJsonBody(new
            {
                type = "message",
                text = input.Message
            });
            return await botClient.ExecuteWithErrorHandling<ChatMessageDto>(botRequest);
        }

        [Action("Send message to chat", Description = "Send message to chat")]
        public async Task<ChatMessageDto> SendMessageToChat([ActionParameter] ChatIdentifier chatIdentifier,
        [ActionParameter] SendMessageRequest input)
        {
            var botClient = new MSTeamsBotClient(input.BotServiceUrl);
            var botRequest = new MSTeamsBotRequest(
                $"v3/conversations/{chatIdentifier.ChatId}/activities",
                Method.Post, _authenticationCredentialsProviders);
            botRequest.AddJsonBody(new
            {
                type = "message",
                text = input.Message
            });
            return await botClient.ExecuteWithErrorHandling<ChatMessageDto>(botRequest);
        }
    }
}
