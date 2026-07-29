namespace Apps.MicrosoftTeamsBot.Constants;

public static class ApplicationConstants
{
    public const string TeamsClientId = "#{MSTEAMS_CLIENT_ID}#";
    public const string TeamsClientSecret = "#{MSTEAMS_SECRET}#";
    public const string TeamsBlackbirdToken = "#{MSTEAMS_BLACKBIRD_TOKEN}#";
    
    public const string TeamsFullScope = TeamsLimitedScope + " ChannelMessage.Read.All";
    public const string TeamsLimitedScope = 
        "offline_access User.Read User.ReadBasic.All Team.ReadBasic.All Channel.ReadBasic.All Chat.Read Files.Read.All Files.ReadWrite";

    public const string BotAppName = "teamsbot";
    public const string BotClientId = "#{MSTEAMSBOT_CLIENT_ID}#";
    public const string BotClientSecret = "#{MSTEAMSBOT_SECRET}#";
}