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
}