using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Factories;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.RestSharp;

namespace Apps.Crowdin.Invocables;

public class AppInvocable(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    private static readonly IApiClientFactory ApiClientFactory = new ApiClientFactory();

    protected List<AuthenticationCredentialsProvider> Creds => InvocationContext.AuthenticationCredentialsProviders.ToList();
    
    protected BlackBirdRestClient RestClient => ApiClientFactory.BuildRestClient(Creds);

    protected CrowdinClient SdkClient => ApiClientFactory.BuildSdkClient(Creds);

    protected void CheckAccessToEnterpriseAction()
    {
        var plan = ApiClientFactory.GetPlan(Creds);

        if (plan == Plans.Basic)
        {
            throw new PluginMisconfigurationException(
                "You are not able to execute this action because your connection is based on the basic plan. " +
                "This action is only available for enterprise-level Crowdin connections.");
        }
    }
}