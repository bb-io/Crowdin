using RestSharp;

namespace Apps.Crowdin.Api.RestSharp.Basic;

public class CrowdinRestClient : RestClient
{
    public CrowdinRestClient() : base(new RestClientOptions { BaseUrl = new("https://api.crowdin.com/api/v2") })
    { }
}