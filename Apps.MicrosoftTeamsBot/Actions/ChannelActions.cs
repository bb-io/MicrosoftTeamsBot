using Apps.MicrosoftTeamsBot.Dtos;
using Apps.MicrosoftTeamsBot.Models.Identifiers;
using Apps.MicrosoftTeamsBot.Models.Requests;
using Apps.MicrosoftTeamsBot.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Files;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Microsoft.Graph;
using Microsoft.Graph.Drives.Item.Items.Item.CreateUploadSession;
using Microsoft.Graph.Models;
using Microsoft.Graph.Models.ODataErrors;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.MicrosoftTeamsBot.Actions;

[ActionList("Channels")]
public class ChannelActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : MsTeamsBotInvocable(invocationContext)
{
    [Action("Reply to message in channel", Description = "Post a reply to an existing message in a channel")]
    public async Task<ChatMessageDto> ReplyToMessageInChannel(
        [ActionParameter] ChannelIdentifier channelIdentifier,
        [ActionParameter] MessageIdentifier messageIdentifier, 
        [ActionParameter] SendMessageRequest input,
        [ActionParameter] ServiceUrlIdentifier urlIdentifier)
    {
        string serviceUrl = urlIdentifier.Resolve();
        string teamChannelId = channelIdentifier.Resolve().ChannelId;
        string endpoint = $"v3/conversations/{teamChannelId};messageid={messageIdentifier.MessageId}/activities/{messageIdentifier.MessageId}";
        
        var botClient = new MSTeamsBotClient(serviceUrl);
        var botRequest = new RestRequest(endpoint, Method.Post)
            .AddStringBody(
                JsonConvert.SerializeObject(new ChannelMessageSendDto()
                {
                    Type = "message",
                    Text = input.AttachmentFile != null ? "" : (input.Message ?? ""),
                    Attachments = await CreateAttachment(input)
                }), DataFormat.Json);
        
        return await botClient.ExecuteWithErrorHandling<ChatMessageDto>(botRequest);
    }

    [Action("Send message to channel", Description = "Post a new message to the specified channel")]
    public async Task<ChatMessageDto> SendMessageToChannel(
        [ActionParameter] ChannelIdentifier channelIdentifier,
        [ActionParameter] SendMessageRequest input,
        [ActionParameter] ServiceUrlIdentifier urlIdentifier)
    {
        string serviceUrl = urlIdentifier.Resolve();
        string teamChannelId = channelIdentifier.Resolve().ChannelId;
        
        var botClient = new MSTeamsBotClient(serviceUrl);
        var botRequest = new RestRequest($"v3/conversations/{teamChannelId}/activities", Method.Post);
        
        botRequest.AddJsonBody(new
        {
            type = "message",
            text = input.Message
        });
        return await botClient.ExecuteWithErrorHandling<ChatMessageDto>(botRequest);
    }

    [Action("Download files attached to channel message", Description = "Download files attached to a channel message")]
    public async Task<DownloadFilesAttachedToMessageResponse> DownloadFilesAttachedToMessage(
        [ActionParameter] ChannelIdentifier channelIdentifier,
        [ActionParameter] MessageIdentifier messageIdentifier)
    {
        var client = new MSTeamsClient(Creds);
        var teamChannel = channelIdentifier.Resolve();

        try
        {
            var message = await client.Teams[teamChannel.TeamId].Channels[teamChannel.ChannelId]
                .Messages[messageIdentifier.MessageId].GetAsync();
            var fileAttachments = message.Attachments.Where(a => a.ContentType == "reference");
            var resultFiles = new List<FileReference>();

            foreach (var attachment in fileAttachments)
            {
                var sharingUrl = attachment.ContentUrl;
                var base64Value = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(sharingUrl));
                var encodedUrl = "u!" + base64Value.TrimEnd('=').Replace('/', '_').Replace('+', '-');
                var fileData = await client.Shares[encodedUrl].DriveItem.GetAsync();
                var fileContent = await client.Shares[encodedUrl].DriveItem.Content.GetAsync();
                var contentBytes = await fileContent.GetByteData();

                using var stream = new MemoryStream(contentBytes);
                var file = await fileManagementClient.UploadAsync(stream, fileData.File.MimeType, fileData.Name);

                resultFiles.Add(file);
            }

            return new DownloadFilesAttachedToMessageResponse { Files = resultFiles.Select(file => new FileDto(file)) };
        }
        catch (ODataError error)
        {
            throw new Exception(error.Error.Message);
        }
    }

    private async Task<List<MessageAttachmentDto>> CreateAttachment(SendMessageRequest input)
    {
        var attachments = new List<MessageAttachmentDto>();
        if (input.AttachmentFile != null)
        {
            var uploadedFile = await UploadFile(input.AttachmentFile);
            attachments.Add(new MessageAttachmentDto()
            {
                ContentType = "application/vnd.microsoft.card.hero",
                Content = new Content()
                {
                    Text = input.Message ?? "",
                    Buttons = new List<Button>()
                    {
                        new Button()
                        {
                            Type = "openUrl",
                            Title = input.AttachmentFile.Name,
                            Value = uploadedFile.WebUrl
                        }
                    }
                }
            });
        }
        return attachments;
    }

    private async Task<DriveItem> UploadFile(FileReference file)
    {
        const string teamsFilesFolderName = "Microsoft Teams Chat Files";
        const int chunkSize = 3932160;

        var client = new MSTeamsClient(InvocationContext.AuthenticationCredentialsProviders);
        var drive = await client.Me.Drive.GetAsync();
        var root = await client.Drives[drive.Id].Root.GetAsync();
        var folders = await client.Drives[drive.Id].Items[root.Id].Children.GetAsync();
        var teamsFilesFolder = folders.Value.FirstOrDefault(folder =>
            folder.Folder is not null && folder.Name == teamsFilesFolderName);

        if (teamsFilesFolder is null)
            teamsFilesFolder = await client.Drives[drive.Id].Items[root.Id].Children.PostAsync(new DriveItem
            {
                Name = teamsFilesFolderName,
                Folder = new Folder()
            });

        
        var uploadSessionRequestBody = new CreateUploadSessionPostRequestBody
        {
            Item = new DriveItemUploadableProperties
            {
                AdditionalData = new Dictionary<string, object>
                {
                    { "@microsoft.graph.conflictBehavior", "rename" }
                }
            }
        };

        var uploadSession = await client.Drives[drive.Id].Items[teamsFilesFolder.Id].ItemWithPath(file.Name)
            .CreateUploadSession.PostAsync(uploadSessionRequestBody);
        using var stream = await fileManagementClient.DownloadAsync(file);
        using var memoryStream = new MemoryStream();
        stream.CopyTo(memoryStream);

        var fileUploadTask = new LargeFileUploadTask<DriveItem>(uploadSession, memoryStream, chunkSize, client.RequestAdapter);
        var uploadResult = await fileUploadTask.UploadAsync();
        return uploadResult.ItemResponse;
    }
}