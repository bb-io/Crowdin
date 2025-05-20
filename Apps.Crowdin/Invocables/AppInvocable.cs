using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Factories;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using System.Reflection;

namespace Apps.Crowdin.Invocables;

public class AppInvocable(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    private static readonly IApiClientFactory ApiClientFactory = new ApiClientFactory();

    protected List<AuthenticationCredentialsProvider> Creds => InvocationContext.AuthenticationCredentialsProviders.ToList();

    //protected RestClient RestClient => ApiClientFactory.BuildRestClient(InvocationContext.AuthenticationCredentialsProviders);

    protected RestClient RestClient
    {
        get
        {
            var orig = ApiClientFactory.BuildRestClient(Creds);

            var httpProp = typeof(RestClient)
                .GetProperty("HttpClient", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public)!;
            var httpClient = (HttpClient)httpProp.GetValue(orig)!;

            var baseUrl = orig.Options.BaseUrl!;

            var options = new RestClientOptions(baseUrl)
            {
                Timeout =TimeSpan.FromMinutes(5),
                FollowRedirects = orig.Options.FollowRedirects,
                Proxy = orig.Options.Proxy,
                AutomaticDecompression = orig.Options.AutomaticDecompression,
            };
            var client = new RestClient(
                httpClient,
                options,
                disposeHttpClient: false
            );

            return client;
        }
    }

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