﻿using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Microsoft.Graph.Models;

namespace Apps.MicrosoftTeamsBot.DynamicHandlers
{
    public class ChatHandler : BaseInvocable, IAsyncDataSourceHandler
    {
        public ChatHandler(InvocationContext invocationContext) : base(invocationContext)
        {    
        }

        public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context, 
            CancellationToken cancellationToken)
        {
            var contextInv = InvocationContext;
            var client = new MSTeamsClient(contextInv.AuthenticationCredentialsProviders);
            var me = await client.Me.GetAsync(cancellationToken: cancellationToken);
            var chats = await client.Me.Chats.GetAsync(requestConfiguration =>
            {
                requestConfiguration.QueryParameters.Expand = new[] { "members" };
                var filter = $"NOT(chatType eq 'meeting') and ((contains(topic, '{context.SearchString ?? ""}') or " +
                             $"(topic eq null and (members/any(x:contains(x/displayName, '{context.SearchString ?? ""}'))))))";
                requestConfiguration.QueryParameters.Filter = filter;
                requestConfiguration.QueryParameters.Orderby = new []{ "lastMessagePreview/createdDateTime desc" };
            }, cancellationToken);
            
            return chats.Value
                .ToDictionary(k => k.Id, v => string.IsNullOrEmpty(v.Topic) 
                    ? v.ChatType == ChatType.OneOnOne 
                        ? v.Members.FirstOrDefault(m => ((AadUserConversationMember)m).UserId != me.Id)?.DisplayName ?? "Unknown user"
                        : string.Join(", ", v.Members.Where(m => ((AadUserConversationMember)m).UserId != me.Id)
                            .Select(m => m.DisplayName)) 
                    : v.Topic);
        }
    }
}
