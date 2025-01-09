using Apps.Crowdin.Api;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Crowdin.Factories;

public interface IApiClientFactory
{
    public string GetPlan(IEnumerable<AuthenticationCredentialsProvider> credentialsProviders);
    
    public RestClient BuildRestClient(IEnumerable<AuthenticationCredentialsProvider> credentialsProviders);
    
    public CrowdinClient BuildSdkClient(IEnumerable<AuthenticationCredentialsProvider> credentialsProviders);
}