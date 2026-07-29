using System.Net;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Newtonsoft.Json;

namespace Apps.MicrosoftTeamsBot.Webhooks.Lists;

public class BaseWebhookList(InvocationContext invocationContext) : MsTeamsBotInvocable(invocationContext)
{
    protected async Task<WebhookResponse<T>> HandleWebhookRequest<T>(WebhookRequest request) where T: class
    {
        var eventPayload = JsonConvert.DeserializeObject<T>(request.Body.ToString(), new JsonSerializerSettings
        {
            MissingMemberHandling = MissingMemberHandling.Ignore
        });
        if (eventPayload is null)
            return new WebhookResponse<T>
            {
                HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
                ReceivedWebhookRequestType = WebhookRequestType.Preflight
            };

        return new WebhookResponse<T>
        {
            HttpResponseMessage = new HttpResponseMessage(HttpStatusCode.OK),
            Result = eventPayload
        };
    }
}