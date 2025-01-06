using Apps.Crowdin.Api;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Crowdin.Api;
using RestSharp;

namespace Apps.Crowdin.Factories;

public class ApiClientFactory : IApiClientFactory
{
    public RestClient BuildRestClient(IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        var crowdinPlan = credentialsProviders.Get(CredsNames.CrowdinPlan).Value;
        
        if (crowdinPlan == Plans.BasicPlan)
        {
            return new CrowdinRestClient();
        }

        if (crowdinPlan == Plans.Enterprise)
        {
            return new CrowdinEnterpriseRestClient(credentialsProviders);
        }

        throw new Exception($"Unsupported crowdin plan provided: {crowdinPlan}");
    }

    public CrowdinClient BuildSdkClient(IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        var authenticationCredentialsProviders = credentialsProviders as AuthenticationCredentialsProvider[] ?? credentialsProviders.ToArray();
        var crowdinPlan = authenticationCredentialsProviders.Get(CredsNames.CrowdinPlan).Value;

        var credentials = new CrowdinCredentials
        {
            AccessToken = authenticationCredentialsProviders.Get(CredsNames.ApiToken).Value
        };

        if (crowdinPlan == Plans.Enterprise)
        {
            credentials.Organization = authenticationCredentialsProviders.Get(CredsNames.OrganizationDomain).Value;
        }
        
        return new CrowdinClient(credentials, authenticationCredentialsProviders);
    }
}