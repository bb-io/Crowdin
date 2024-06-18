using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api;

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