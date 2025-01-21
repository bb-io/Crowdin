using Apps.Crowdin.Api.RestSharp.Enterprise;
using Blackbird.Applications.Sdk.Common.Authentication;
using RestSharp;

namespace Tests.Crowdin
{
    public class TestCrowdinEnterpriseRestClient : CrowdinEnterpriseRestClient
    {
        public TestCrowdinEnterpriseRestClient(IEnumerable<AuthenticationCredentialsProvider> creds)
            : base(creds) { }

        public Exception PublicConfigureErrorException(RestResponse response)
        {
            return base.ConfigureErrorException(response);
        }
    }
}
