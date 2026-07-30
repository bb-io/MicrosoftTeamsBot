using Blackbird.Applications.Sdk.Common;

namespace Apps.MicrosoftTeamsBot.Models.Identifiers;

public class ServiceUrlIdentifier
{
    [Display("Bot service url", Description = 
        "Leave blank unless messages fail to send - the default works for Microsoft 365 commercial tenants. " +
        "If needed, map this URL from an event output.")]
    public string? BotServiceUrl { get; set; }

    public string Resolve()
    {
        return string.IsNullOrWhiteSpace(BotServiceUrl) ? "https://smba.trafficmanager.net/teams/" : BotServiceUrl;
    }
}