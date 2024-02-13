using Apps.MicrosoftTeamsBot.DynamicHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.MicrosoftTeamsBot.Webhooks.Inputs;

public class SenderInput
{
    [Display("Sender")]
    [DataSource(typeof(UserHandler))]
    public string? UserId { get; set; }
}