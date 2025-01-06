using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Apps.Crowdin.Api.RestSharp.Basic;

public class CrowdinRestRequest : RestRequest
{
    public CrowdinRestRequest(
        string source,
        Method method,
        IEnumerable<AuthenticationCredentialsProvider> creds) : base(source, method)
    {
        var token = creds.First(x => x.KeyName == CredsNames.ApiToken).Value;
        this.AddHeader("Authorization", $"Bearer {token}");
    }
}