namespace Apps.MicrosoftTeamsBot.Constants;

public static class ConnectionTypes
{
    public const string OAuth = "OAuth";
    public const string OAuthCustomApp = "OAuthCustomApp";

    public static readonly IEnumerable<string> SupportedConnectionTypes = [OAuth, OAuthCustomApp];
}
