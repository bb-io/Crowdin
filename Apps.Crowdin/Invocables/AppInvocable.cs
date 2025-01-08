using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Factories;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;

namespace Apps.Crowdin.Invocables;

public class AppInvocable(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    private static readonly IApiClientFactory ApiClientFactory = new ApiClientFactory();

    protected List<AuthenticationCredentialsProvider> Creds => InvocationContext.AuthenticationCredentialsProviders.ToList();

    protected RestClient RestClient => ApiClientFactory.BuildRestClient(InvocationContext.AuthenticationCredentialsProviders);

    protected CrowdinClient SdkClient =>
        ApiClientFactory.BuildSdkClient(InvocationContext.AuthenticationCredentialsProviders);

    protected void CheckAccessToEnterpriseAction()
    {
        var plan = ApiClientFactory.GetPlan(InvocationContext.AuthenticationCredentialsProviders);

        if (plan == Plans.Basic)
        {
            throw new PluginMisconfigurationException(
                "You are not able to execute this action because your connection is based on the basic plan. " +
                "This action is only available for enterprise-level Crowdin connections.");
        }
    }
}