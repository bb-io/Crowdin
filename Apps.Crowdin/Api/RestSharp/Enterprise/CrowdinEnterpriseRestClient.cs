using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Blackbird.Applications.Sdk.Utils.Extensions.String;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Api.RestSharp.Enterprise;

public class CrowdinEnterpriseRestClient(IEnumerable<AuthenticationCredentialsProvider> creds)
    : BlackBirdRestClient(GetRestClientOptions(creds))
{
    private static RestClientOptions GetRestClientOptions(IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var domain = creds.Get(CredsNames.OrganizationDomain).Value;
        
        return new()
        {
            BaseUrl = $"https://{domain}.api.crowdin.com/api/v2".ToUri()
        };
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (string.IsNullOrEmpty(response.Content))
        {
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new Exception("Response and error message is empty.");
            }

            throw new PluginApplicationException(response.ErrorMessage);
        }
        
        var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content!)!;
        throw new PluginMisconfigurationException($"Code: {error.Error.Code}; Message: {error.Error.Message}");
    }
}