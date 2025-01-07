using RestSharp;

namespace Apps.Crowdin.Api.RestSharp.Basic;

public class CrowdinRestClient() : RestClient(new RestClientOptions { BaseUrl = new("https://api.crowdin.com/api/v2") });