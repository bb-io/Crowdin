using Apps.Crowdin.Api;
using Apps.Crowdin.Factories;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
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
}