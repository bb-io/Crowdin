using Apps.Crowdin.Models.Request.TranslationMemory;
using Crowdin.Api.StringTranslations;
using Crowdin.Api.TranslationMemory;
using RestSharp;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class TranslationMemoryTests : TestBase
    {
        [TestMethod]
        public async Task ListTranslationMemories_ShouldReturnResults()
        {
            var action = new Apps.Crowdin.Actions.TranslationMemoryActions(InvocationContext, FileManager);
            var request = new Apps.Crowdin.Models.Request.TranslationMemory.ListTranslationMemoryRequest
            {
                UserId = null,
                GroupId = null
            };

            var response = await action.ListTranslationMemories(request);

            foreach (var tm in response.TranslationMemories)
            {
                Console.WriteLine($"TM ID: {tm.Id}, Name: {tm.Name}");
            }

            Assert.IsNotNull(response);
            Assert.IsNotNull(response.TranslationMemories);
            Assert.IsTrue(response.TranslationMemories.Length > 0, "Expected at least one translation memory.");
        }

        [TestMethod]
        public async Task GetTranslationMemory_ShouldReturnResults()
        {
            var action = new Apps.Crowdin.Actions.TranslationMemoryActions(InvocationContext, FileManager);
            var request = new Apps.Crowdin.Models.Request.TranslationMemory.TranslationMemoryRequest
            {
                TranslationMemoryId = "10"
            };

            var response = await action.GetTranslationMemory(request);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task AddTranslationMemory_ShouldReturnResults()
        {
            var action = new Apps.Crowdin.Actions.TranslationMemoryActions(InvocationContext, FileManager);
            var request = new Apps.Crowdin.Models.Request.TranslationMemory.AddTranslationMemoryRequest
            {
                Name = "Test TM from API 2",
                LanguageId = "en",
            };

            var response = await action.AddTranslationMemory(request);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task ImportTranslationMemory_ShouldReturnResults()
        {
            var action = new Apps.Crowdin.Actions.TranslationMemoryActions(InvocationContext, FileManager);
            var req1 = new TranslationMemoryRequest { TranslationMemoryId= "10" };
            var request = new Apps.Crowdin.Models.Request.TranslationMemory.ImportTranslationMemoryRequest
            {
                File = new Blackbird.Applications.Sdk.Common.Files.FileReference { 
                Name= "Konference's TM.tmx"
                }
            };

            var response = await action.ImportTranslationMemory(req1, request);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task SearchTmSegments_ShouldReturnResults()
        {
            var action = new Apps.Crowdin.Actions.TranslationMemoryActions(InvocationContext, FileManager);
            var req1 = new TranslationMemoryRequest { TranslationMemoryId = "10" };
            var req2= new Apps.Crowdin.Models.Request.TranslationMemory.SearchTmSegmentsRequest
            {
                LanguageId = "en"
            };

            var response = await action.SearchTmSegments(req1, req2);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task GetTmSegment_ShouldReturnResults()
        {
            var action = new Apps.Crowdin.Actions.TranslationMemoryActions(InvocationContext, FileManager);
            var req1 = new TranslationMemoryRequest { TranslationMemoryId = "10" };
            var req2 = new Apps.Crowdin.Models.Request.TranslationMemory.TmSegmentRequest
            {
                SegmentId = "667"
            };

            var response = await action.GetTmSegment(req1, req2);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));

            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task DeleteTmSegment_ShouldReturnResults()
        {
            var action = new Apps.Crowdin.Actions.TranslationMemoryActions(InvocationContext, FileManager);
            var req1 = new TranslationMemoryRequest { TranslationMemoryId = "10" };
            var req2 = new Apps.Crowdin.Models.Request.TranslationMemory.TmSegmentRequest
            {
                SegmentId = "669"
            };

            await action.DeleteTmSegment(req1, req2);
            Assert.IsTrue(true);
        }

        //EditTmSegment

        [TestMethod]
        public async Task EditTmSegment_ShouldReturnResults()
        {
            var action = new Apps.Crowdin.Actions.TranslationMemoryActions(InvocationContext, FileManager);
            var req1 = new TranslationMemoryRequest { TranslationMemoryId = "10" };
            var req2 = new Apps.Crowdin.Models.Request.TranslationMemory.TmSegmentRequest
            {
                SegmentId = "667"
            };
            var req3 = new Apps.Crowdin.Models.Request.TranslationMemory.EditTmSegmentRequest
            {
                LanguageId= "en",
                Operation = "add",
                //RecordId = "1540",
                Text = ""
            };

            var response = await action.EditTmSegment(req1, req2, req3);
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));

            Assert.IsNotNull(response);
        }
    }
}
