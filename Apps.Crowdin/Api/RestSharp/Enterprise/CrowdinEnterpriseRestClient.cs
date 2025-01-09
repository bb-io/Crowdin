using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using RestSharp;

namespace Apps.Crowdin.Api.RestSharp.Enterprise;

public class CrowdinEnterpriseRestClient(IEnumerable<AuthenticationCredentialsProvider> creds)
    : RestClient(GetRestClientOptions(creds))
{
    private static RestClientOptions GetRestClientOptions(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var domain = creds.Get(CredsNames.OrganizationDomain).Value;
        
        return new()
        {
            BaseUrl = $"https://{domain}.api.crowdin.com/api/v2".ToUri()
        };
    }
}