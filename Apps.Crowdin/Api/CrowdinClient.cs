using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Files;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using Crowdin.Api;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Api;

public class CrowdinClient : CrowdinApiClient
{
    private readonly AuthenticationCredentialsProvider[] _creds;
    
    public CrowdinClient(IEnumerable<AuthenticationCredentialsProvider> creds)
        : base(GetCrowdinCreds(creds))
    {
        this._creds = creds.ToArray();
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

    public async Task<StorageResourceDto> AddStorageAsync(string fileName, Stream fileStream)
    {
        var credentials = GetCrowdinCreds(_creds);
        var baseUrl = GetBaseUrl(credentials);
        
        await Logger.LogAsync(new
        {
            Credentials = credentials,
        });
        
        using var memoryStream = new MemoryStream();
        await fileStream.CopyToAsync(memoryStream);
        var bytes = memoryStream.ToArray();

        var restRequest = new RestRequest("/storages", Method.Post)
            .AddHeader("Crowdin-API-FileName", Uri.EscapeDataString(fileName))
            .AddHeader("Authorization", $"Bearer {credentials.AccessToken}")
            .AddHeader("Content-Type", "application/octet-stream");
        
        restRequest.AddParameter("application/octet-stream", bytes, ParameterType.RequestBody);

        var response = await ExecuteRequestAsync<DataWrapper<StorageResourceDto>>(restRequest, baseUrl);
        return response.Data;
    }

    public async Task<FileDto> AddFileAsync(int projectId, AddFileRequestDto body)
    {
        var credentials = GetCrowdinCreds(_creds);
        var baseUrl = GetBaseUrl(credentials);
        
        await Logger.LogAsync(new
        {
            Credentials = credentials,
        });
        
        var restClient = new RestClient(baseUrl);
        var restRequest = new RestRequest($"/projects/{projectId}/files", Method.Post)
            .AddHeader("Authorization", $"Bearer {credentials.AccessToken}")
            .WithJsonBody(body);
        
        var response = await ExecuteRequestAsync<DataWrapper<FileDto>>(restRequest, baseUrl);
        return response.Data;
    }
    
    private static string GetBaseUrl(CrowdinCredentials credentials)
    {
        return string.IsNullOrWhiteSpace(credentials.BaseUrl)
            ? (string.IsNullOrWhiteSpace(credentials.Organization) ? "https://api.crowdin.com/api/v2" : $"https://{credentials.Organization}.api.crowdin.com/api/v2")
            : credentials.BaseUrl;
    }
    
    private static async Task<T> ExecuteRequestAsync<T>(RestRequest request, string baseUrl) where T : class
    {
        var restClient = new RestClient(baseUrl);
        var response = await restClient.ExecuteAsync(request);

        if (response.IsSuccessful)
        {
            return JsonConvert.DeserializeObject<T>(response.Content!)!;
        }

        throw new Exception($"Request failed: {response.Content}; Status code: {response.StatusCode}");
    }
}