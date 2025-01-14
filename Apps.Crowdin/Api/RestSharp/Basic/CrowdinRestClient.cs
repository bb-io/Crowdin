using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Api.RestSharp.Basic;

public class CrowdinRestClient() : BlackBirdRestClient(new RestClientOptions { BaseUrl = new("https://api.crowdin.com/api/v2") })
{
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