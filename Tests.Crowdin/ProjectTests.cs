using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Connections;
using Apps.Crowdin.Models.Request.Project;
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
                ProjectId = "750225"
            };

            var client = new ProjectActions(InvocationContext, FileManager);

            var result = await client.GetProject(input);

            Assert.IsNotNull(result);
        }
    }
}
