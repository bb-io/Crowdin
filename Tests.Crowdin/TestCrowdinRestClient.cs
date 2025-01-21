using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.Api.RestSharp.Basic;
using RestSharp;

namespace Tests.Crowdin
{
    public class TestCrowdinRestClient : CrowdinRestClient
    {
        public TestCrowdinRestClient() { }

        public Exception PublicConfigureErrorException(RestResponse response)
        {
            return base.ConfigureErrorException(response);
        }
    }
}
