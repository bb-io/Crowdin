using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Response.Glossaries;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Crowdin.Api;
using Crowdin.Api.Glossaries;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Api;

public class CrowdinClient(CrowdinCredentials credentials, AuthenticationCredentialsProvider[] creds)
    : CrowdinApiClient(credentials,new HttpClient { Timeout=TimeSpan.FromMinutes(5)})
{
    public async Task<ExportGlossaryModel> ExportGlossaryAsync(int glossaryId, string format)
    {
        var plan = creds.Get(CredsNames.CrowdinPlan)?.Value
                   ?? throw new PluginMisconfigurationException("Missing crowdin plan");

        string baseUrl;
        if (plan.Equals(Plans.Basic, StringComparison.OrdinalIgnoreCase))
        {
            baseUrl = "https://api.crowdin.com/api/v2";
        }
        else
        {
            var orgCred = creds.Get(CredsNames.OrganizationDomain);
            var organization = orgCred.Value;
            baseUrl = $"https://{organization}.api.crowdin.com/api/v2";
        }

        var token = creds.Get(CredsNames.ApiToken)?.Value
                    ?? throw new PluginApplicationException("Missing credential: access token");

        var options = new RestClientOptions
        {
            BaseUrl = new Uri(baseUrl),
            Timeout = TimeSpan.FromSeconds(180)
        };

        var restClient = new RestClient(options);
        var restRequest = new RestRequest($"/glossaries/{glossaryId}/exports", Method.Post)
            .WithHeaders(new Dictionary<string, string>
            {
                ["Authorization"] = $"Bearer {token}"
            })
            .WithJsonBody(new { format = format });

        var exportResponse = await restClient.ExecuteAsync<GlossaryExportStatus>(restRequest);
        var data = JsonConvert
            .DeserializeObject<GlossaryExportResponse>(exportResponse.Content!)!
            .Data;

        return data;
    }
}