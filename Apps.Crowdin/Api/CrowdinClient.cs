using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Response.Glossaries;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Crowdin.Api;
using Crowdin.Api.Glossaries;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Api;

public class CrowdinClient : CrowdinApiClient
{
    private readonly AuthenticationCredentialsProvider[] _creds;
    
    public CrowdinClient(IEnumerable<AuthenticationCredentialsProvider> creds)
        : base(GetCrowdinCreds(creds))
    {
        _creds = creds.ToArray();
    }

    public CrowdinClient(CrowdinCredentials credentials, AuthenticationCredentialsProvider[] creds) : base(credentials)
    {
        _creds = creds;
    }

    private static CrowdinCredentials GetCrowdinCreds(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var token = creds.First(x => x.KeyName == CredsNames.ApiToken);

        return new()
        {
            AccessToken = token.Value
        };
    }
    
    public async Task<ExportGlossaryModel> ExportGlossaryAsync(int glossaryId)
    {
        var organization = _creds.Get(CredsNames.OrganizationDomain).Value;
        var token = _creds.Get(CredsNames.ApiToken).Value;
        
        var restClient = new RestClient($"https://{organization}.api.crowdin.com/api/v2");
        
        var restRequest = new RestRequest($"/glossaries/{glossaryId}/exports", Method.Post)
            .WithHeaders(new Dictionary<string, string> { {"Authorization", $"Bearer {token}"}})
            .WithJsonBody(new { format = "tbx" });
    
        var exportGlossary = await restClient.ExecuteAsync<GlossaryExportStatus>(restRequest);

        var data = JsonConvert.DeserializeObject<GlossaryExportResponse>(exportGlossary.Content!)!;
        return data.Data;
    }
}