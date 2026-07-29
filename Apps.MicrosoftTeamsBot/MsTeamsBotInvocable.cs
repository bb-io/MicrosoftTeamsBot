using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.MicrosoftTeamsBot;

public class MsTeamsBotInvocable : BaseInvocable
{
    protected AuthenticationCredentialsProvider[] Creds => InvocationContext.AuthenticationCredentialsProviders.ToArray();

    protected MsTeamsBotInvocable(InvocationContext invocationContext) : base(invocationContext) { }
}