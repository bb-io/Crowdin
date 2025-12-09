using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Threading.Tasks;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class DataHandlerTests : TestBase
    {
        [TestMethod]
        public async Task FileDataHandler_IsSuccess()
        {
            var handler = new FileDataHandler(
                InvocationContext,
                new ProjectRequest
                {
                    ProjectId = "783572"
                });
            var response = await handler.GetDataAsync(
                new DataSourceContext
                {
                },
                CancellationToken.None);

            foreach (var file in response)
            {
                Console.WriteLine($"File ID: {file.Value}, Name: {file.DisplayName}");
            }
            Assert.IsNotNull(response);  
        }


        [TestMethod]
        public async Task DirectoryDataHandler_IsSuccess()
        {
            var handler = new DirectoryDataHandler(
                InvocationContext,
                new ProjectRequest
                {
                    ProjectId = "5"
                });
            var response = await handler.GetDataAsync(
                new DataSourceContext
                {
                },
                CancellationToken.None);

            foreach (var file in response)
            {
                Console.WriteLine($"Directory ID: {file.Value}, Name: {file.DisplayName}");
            }
            Assert.IsNotNull(response);
        }

        [TestMethod]
        public async Task BracnhesDataHandler_IsSuccess()
        {
            var handler = new BranchDataHandler(
                InvocationContext,
                new ProjectRequest
                {
                    ProjectId = "783572"
                });
            var response = await handler.GetDataAsync(
                new DataSourceContext
                {
                },
                CancellationToken.None);

            foreach (var file in response)
            {
                Console.WriteLine($"File ID: {file.Value}, Name: {file.DisplayName}");
            }
            Assert.IsNotNull(response);
        }

       

        [TestMethod]
        public async Task TargetLanguageDataHandler_IsSuccess()
        {
            var handler = new ProjectTargetLanguageDataHandler(
                InvocationContext,
                new ProjectRequest
                {
                    ProjectId = "783572"
                });
            var response = await handler.GetDataAsync(
                new DataSourceContext
                {
                },
                CancellationToken.None);

            foreach (var file in response)
            {
                Console.WriteLine($"Lang ID: {file.Value}, Name: {file.DisplayName}");
            }
            Assert.IsNotNull(response);
        }
    }
}
