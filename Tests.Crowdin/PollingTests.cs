using Apps.Crowdin.Polling.Models;
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
                    //TranslationMemoryId = "10",
                    //ImportId = "0bb5e79d-890e-464f-8b11-8411b8bb3f9c",
                    //Statuses = new List<string> { "finished", "failed" } 
                    TranslationMemoryId = "666940",
                    ImportId = "c8d6a817-d0a9-4c97-81d7-4087fbdb2464",
                    Statuses = new List<string> { "finished"}
                });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(json);
        }

        [TestMethod]
        public async Task OnAllTasksReachedStatus_IsSuccess()
        {
            var polling = new Apps.Crowdin.Polling.TMExportPollingList(InvocationContext);

            //var result = await polling.OnAllTasksReachedStatus(
            //    new Blackbird.Applications.Sdk.Common.Polling.PollingEventRequest<TasksPollingMemory>
            //    {
            //        Memory = new TasksPollingMemory
            //        {
            //            LastPollingTime = DateTime.UtcNow.AddHours(-1),
            //            Triggered = false
            //        }
            //    },
            //    new Apps.Crowdin.Polling.Models.Requests.AllTasksReachedStatusRequest
            //    {
            //        ProjectId = "3",
            //        Status = new List<string> { "done" },
            //        TitleContains = "Translate"
            //    });
            var result = await polling.OnAllTasksReachedStatus(
                new Blackbird.Applications.Sdk.Common.Polling.PollingEventRequest<TasksPollingMemory>
                {
                },
                new Apps.Crowdin.Polling.Models.Requests.AllTasksReachedStatusRequest
                {
                    ProjectId = "108",
                    Status = new List<string> { "done" },
                    TitleContains = "Blog_Posts_2025_12_level-up-msp-client-conversations"
                });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result);
            Console.WriteLine(json);
            Assert.IsNotNull(json);
        }
    }
}
