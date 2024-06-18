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
}