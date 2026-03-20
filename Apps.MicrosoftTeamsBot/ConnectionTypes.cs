namespace Apps.MicrosoftTeamsBot;

public static class ConnectionTypes
{
    public const string OAuth = "OAuth";
    public const string OAuthCustomApp = "OAuthCustomApp";
    public const string Application = "Application";

    public static readonly IEnumerable<string> SupportedConnectionTypes =
    [
        OAuth,
        OAuthCustomApp,
        Application
    ];
}
