﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Connections;
using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class ProjectTests :TestBase
    {
        [TestMethod]
        public async Task AddProject_ShoudBeSuccess()
        {
            var input = new AddNewProjectRequest
            {
                Name= "test",
                SourceLanguageId = "en"
            };

            var client = new ProjectActions(InvocationContext,FileManager);

            var result = await client.AddProject(input);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetProject_ShoudBeSuccess()
        {
            var input = new ProjectRequest
            {
                ProjectId = "19"
            };

            var client = new ProjectActions(InvocationContext, FileManager);

            var result = await client.GetProject(input);

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task GetProjects_ShoudBeSuccess()
        {
            var handler = new ProjectDataHandler(InvocationContext);  

            var result = await handler.GetDataAsync(new DataSourceContext { SearchString =""}, CancellationToken.None);

            foreach (var item in result)
            {
                Console.WriteLine($"{item.DisplayName} - {item.Value} - ");
                Assert.IsNotNull(result);
            }
        }


        [TestMethod]
        public async Task GenerateTranslationReport_ShoudBeSuccess()
        {
            var action = new ProjectActions(InvocationContext, FileManager);

            var result = await action.GenerateTranslateCostReport(new ProjectRequest
            {
                ProjectId = "2"
            }, new GenerateTranslationCostReportOptions {

            });
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task GenerateCostReport_ShoudBeSuccess()
        {
            var action = new ProjectActions(InvocationContext, FileManager);

            var result = await action.GenerateCostReport(new ProjectRequest
            {
                ProjectId = "2"
            }, new GenerateEstimateCostReportOptions
            {
                Currency = "EUR",
            });

            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            Assert.IsNotNull(result);

        }

        [TestMethod]
        public async Task GenerateTaksCostReport_ShoudBeSuccess()
        {
            var action = new ProjectActions(InvocationContext, FileManager);

            var result = await action.GenerateCostReportByTask(new ProjectRequest
            {
                ProjectId = "2"
            }, new GenerateEstimateCostReportByTaskOptions
            {
                TaskId = "4944",
                //LanguageIds = new[] { "en", "en-BZ" },
            });


          var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            Assert.IsNotNull(result);

        }


        [TestMethod]
        public async Task GenerateTaskTranslationReport_ShoudBeSuccess()
        {
            var action = new ProjectActions(InvocationContext, FileManager);

            var result = await action.GenerateTranslateCostReportByTask(new ProjectRequest
            {
                ProjectId = "2"
            }, new GenerateTranslationCostReportByTaskOptions
            {
                TaskId = "4944",
                //BaseFullTranslations = 0.10f,
                //BaseProofRead = 0.05f,
                //TmMatchType = "perfect",
                //TmPrice = 0.0f,
                //MtMatchType = "100",
                //MtPrice = 0.0f,
                //SuggestMatchType = "100",
                //SuggestPrice = 0.0f,
                //FromDate = DateTime.UtcNow.AddDays(-30).ToUniversalTime(),
                //ToDate = DateTime.UtcNow.ToUniversalTime()
            });


            var json = Newtonsoft.Json.JsonConvert.SerializeObject(result, Newtonsoft.Json.Formatting.Indented);
            Console.WriteLine(json);
            Assert.IsNotNull(result);

        }
    }
}
