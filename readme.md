# Blackbird.io Microsoft 365 Teams Bot

Blackbird is the new automation backbone for the language technology industry. Blackbird provides enterprise-scale automation and orchestration with a simple no-code/low-code platform. Blackbird enables ambitious organizations to identify, vet and automate as many processes as possible. Not just localization workflows, but any business and IT process. This repository represents an application that is deployable on Blackbird and usable inside the workflow editor.

## Introduction

<!-- begin docs -->

Microsoft 365 Teams is a collaborative communication platform that combines chat, video conferencing and file sharing.
This app lets Blackbird participate in Teams conversations as a bot: it sends and replies to messages under its own identity
rather than on behalf of a signed-in user.

## Before setting up

Before you can connect you need to make sure that you have a Microsoft 365 account.

You also need to install the Blackbird bot in Microsoft Teams. 
Download`blackbird-teams-bot.zip` from [this link](https://github.com/bb-io/MicrosoftTeamsBot/blob/develop/blackbird-teams-bot.zip), then:

1. In Teams, go to **Apps** (the **+** icon in the left sidebar) → **Manage your apps** → **Upload an app** → **Upload a custom app**.
2. Select the downloaded `.zip` file.
3. Use the **Add** dropdown to choose **Add to a team** or **Add to a chat** and select every team or chat where the bot should send messages.

> **Note:** If **Upload a custom app** isn't shown, your Teams administrator has 
disabled custom app uploads. Ask them to enable it or to upload the package
for your whole organization via Teams admin center → **Teams apps** → **Manage apps** → **Upload new app**.

The bot can only post to conversations it belongs to. 

### Custom Azure app

If you want to use a custom Azure application for authorization, you should do the following:

- Go to Azure → _App registrations_ → _New registration_.
- Enter the name of your application and choose the supported account types. Click _Register_.
- Go to _Manage_ → _API permissions_ and select the required scopes:
    * _If you don't need to manage channel messages_, select the following:
      offline_access User.Read User.ReadBasic.All Team.ReadBasic.All Channel.ReadBasic.All Chat.Read Files.Read.All Files.ReadWrite.
    * _If you need to manage channel messages_, select the above permissions + the ChannelMessage.Read.All scope.
- Go to _Manage_ → _Authentication_ and click _Add a platform_. Choose _Web_ and enter this redirect URI: https://bridge.blackbird.io/api/AuthorizationCode
- Go to _Manage_ → _Certificates & secrets_ and click _New client secret_. Enter a description (optional) and set the expiration time. **Copy the created secret value, as you won't be able to see it again**.
- Go to _Overview_ and copy the _Application (client) ID_ and _Directory (tenant) ID_. Use these values to register your connection in Blackbird.

## Connecting

### OAuth2

1. Under _Channel messages scope required_ specify _Yes_ if you want to use webhooks and/or actions for channel messages and _No_ if not.
2. Click _Authorize connection_.
3. Follow the instructions that Microsoft gives you, authorizing Blackbird.io to act on your behalf.
4. When you return to Blackbird, confirm that the connection has appeared and the status is _Connected_.

![Connecting using OAuth2](image/README/oauth.png)

### OAuth2 (Azure app)

1. Under _Channel messages scope required_ specify _Yes_ if you want to use webhooks and/or actions for channel messages and _No_ if not.
2. Enter the _Application (client) ID_, _Directory (tenant) ID_ and _Client secret_ obtained from your custom Azure app.
3. Follow the instructions that Microsoft gives you, authorizing Blackbird.io to act on your behalf.
4. When you return to Blackbird, confirm that the connection has appeared and the status is _Connected_.

![Connecting using Azure app](image/README/oauth-azure.png)

## Actions

> **Note**: Every action requires a *Bot service url*. Map the *Service url* output from an event when the action follows a trigger.

### Channels

- **Reply to message in channel**. Post a reply to an existing message in a channel
- **Send message to channel**. Post a new message to the specified channel
- **Download files attached to channel message**. Download files attached to a channel message

### Chats

- **Reply to message in chat**. Post a reply to an existing message in a chat
- **Send message to chat**. Post a new message to the specified chat

## Events

### Channels

- **On bot mentioned in channel**

### Chats

- **On bot mentioned in chat**

## Feedback

Do you want to use this app or do you have feedback on our implementation? Reach out to us using the [established channels](https://www.blackbird.io/) or create an issue.

<!-- end docs -->
