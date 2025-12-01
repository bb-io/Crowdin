using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class PollingTests : TestBase
    {
        [TestMethod]
        public async Task ImportTM_IsSuccess()
        {
            var polling = new Apps.Crowdin.Polling.TMExportPollingList(InvocationContext);

            var result = await polling.OnTmImportStatusChanged(
                new Blackbird.Applications.Sdk.Common.Polling.PollingEventRequest<Apps.Crowdin.Polling.Models.PollingMemory>
                {
                },
                new Apps.Crowdin.Polling.Models.Requests.TranslationMemoryImportStatusChangedRequest
                {
                    TranslationMemoryId = "10",
                    ImportId = "0bb5e79d-890e-464f-8b11-8411b8bb3f9c",
                    Statuses = new List<string> { "finished", "failed" }
                });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(json);
        }
    }
}
