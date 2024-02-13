using Apps.MicrosoftTeamsBot.Webhooks.Inputs;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Microsoft.Graph.Models;

namespace Apps.MicrosoftTeamsBot.Webhooks.Handlers;

public abstract class BaseWebhookHandler : BaseInvocable, IWebhookEventHandler
{
    private string BridgeWebhooksUrl = ""; 
    
    private readonly string _subscriptionEvent;

    protected string SubscriptionId { get; set; }

    protected BaseWebhookHandler(InvocationContext invocationContext, string subscriptionEvent) : base(invocationContext)
    {
        _subscriptionEvent = subscriptionEvent;
        BridgeWebhooksUrl = InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/') + $"/webhooks/{ApplicationConstants.BotAppName}";
    }

    protected BaseWebhookHandler(InvocationContext invocationContext, string subscriptionEvent, [WebhookParameter(true)] string subscriptionId) : this(invocationContext, subscriptionEvent)
    {
        SubscriptionId = subscriptionId;
    }

    public async Task SubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        Dictionary<string, string> values)
    {
        var bridgeService = new BridgeService("https://1e7b-178-211-106-141.ngrok-free.app/api");//InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/'));
        await bridgeService.Subscribe(values["payloadUrl"], SubscriptionId, _subscriptionEvent);
    }

    public async Task UnsubscribeAsync(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders,
        Dictionary<string, string> values)
    {
        var bridgeService = new BridgeService("https://1e7b-178-211-106-141.ngrok-free.app/api");//InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/'));
        var webhooksLeft = await bridgeService.Unsubscribe(values["payloadUrl"], SubscriptionId, _subscriptionEvent);
    }
}