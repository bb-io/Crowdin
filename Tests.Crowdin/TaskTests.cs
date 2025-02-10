using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.File;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Task;
using Apps.Crowdin.Models.Request.Users;
using Blackbird.Applications.Sdk.Common.Invocation;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class TaskTests :TestBase
    {
        [TestMethod]
        public async Task CreateTask_ReturnSucces()
        {
            var action = new TaskActions(InvocationContext,FileManager);

            var input1 = new AssigneesRequest { ProjectId = "750225" };
            var input2 = new AddNewTaskRequest { Type = "Translate", Title="Hello", LanguageId = "nl", FileIds = ["16"], Status = "todo" };

            var result = await action.AddTask(input1,input2);

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetFile_IsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);

            var input1 = new ProjectRequest { ProjectId = "750225" };
            var input2 = new FileRequest { FileId = "10"};

            var response = await action.GetFile(input1, input2);

            Assert.IsNotNull(response);
        }


        [TestMethod]
        public async Task ListFiles_IsSuccess()
        {
            var action = new FileActions(InvocationContext, FileManager);

            var input1 = new ProjectRequest { ProjectId = "750225" };
            var input2 = new ListFilesRequest {};

            var response = await action.ListFiles(input1, input2);

            foreach (var file in response.Files)
            {
                Console.WriteLine(file.Id);
            }
        }
    }
}
