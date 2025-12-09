using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.Directory;
using Apps.Crowdin.Models.Request.Project;
using Crowdin.Api.SourceFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class DirectoryTests : TestBase
    {
        [TestMethod]
        public async Task AddDirectory_IsSucces()
        {
            var action = new DirectoryActions(InvocationContext);

            var project = new ProjectRequest
            {
                ProjectId = "776754",
            };

            var request = new AddNewDirectoryRequest
            {
                Path = "/root/Home/Sitemap/Data/Sitemap/Column 1/Home",
                //Path = "/root/Home/Sitemap/Data/Sitemap2/Column 1/Home2",
                PathContainsFile = true
            };

            var response = await action.AddDirectory(project, request);

            Console.WriteLine(response.Name);
        }

        [TestMethod]
        public async Task GetDirectoryProgress_IsSucces()
        {
            var action = new DirectoryActions(InvocationContext);

            var project = new ProjectRequest
            {
                ProjectId = "1",
            };

            var request = new DirectoryRequest
            {
                DirectoryId = "413"
            };

            var response = await action.GetDirectoryProgress(project, request);

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(response));
            Assert.IsNotNull(response);
        }
    }
}