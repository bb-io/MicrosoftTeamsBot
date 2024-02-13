using System.Text.RegularExpressions;
using Apps.MicrosoftTeamsBot.Webhooks.Payload;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.MicrosoftTeamsBot.Webhooks.Lists.ItemGetters;

public abstract class ItemGetter<T>
{
    protected readonly IEnumerable<AuthenticationCredentialsProvider> AuthenticationCredentialsProviders;
    
    protected ItemGetter(IEnumerable<AuthenticationCredentialsProvider> authenticationCredentialsProviders)
    {
        AuthenticationCredentialsProviders = authenticationCredentialsProviders;
    }
    
    public abstract Task<T?> GetItem(EventPayload eventPayload);
    
    protected static string GetIdFromEndpoint(string endpoint, string itemName)
    {
        string pattern = $"{itemName}\\('([^']*)'\\)";
        Match match = Regex.Match(endpoint, pattern);
        return match.Groups[1].Value;
    }
}