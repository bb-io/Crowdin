using Apps.Crowdin.Invocables;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Crowdin.Actions;

[ActionList]
public class DebugActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("[Debug] Action", Description = "Debug action")]
    public List<AuthenticationCredentialsProvider> DebugAction() => Creds.ToList();
}