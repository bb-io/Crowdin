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
using Crowdin.Api.ProjectsGroups;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class TaskTests : TestBase
    {
        [TestMethod]
        public async Task CreateTask_ReturnSucces()
        {
            var action = new TaskActions(InvocationContext, FileManager);

            var input1 = new AssigneesRequest { ProjectId = "750225" };
            var input2 = new AddNewTaskRequest { Type = "Translate", Title = "Hello", LanguageId = "nl", FileIds = ["16"], Status = "todo" };

            var result = await action.AddTask(input1, input2);

            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task ListTask_ReturnSucces()
        {
            var action = new TaskActions(InvocationContext, FileManager);

            var input1 = new ListTasksRequest { ProjectId = "750225" };

            var result = await action.ListTasks(input1);

            foreach (var task in result.Tasks)
            {
                Console.WriteLine(task.Id);
                Assert.IsNotNull(result);
            }
        }


        [TestMethod]
        public async Task GetTask_ReturnSucces()
        {
            var action = new TaskActions(InvocationContext, FileManager);

            var input1 = new ProjectRequest { ProjectId = "750225" };
            string input2 = "4";
            var result = await action.GetTask(input1, input2);

            Console.WriteLine($"{result.Id} - {result.Title} - {result.ProjectId} - {result.Description} - {result.Deadline}");

            foreach (var id in result.FileIds)
            {
                Console.WriteLine(id);
            }
            Assert.IsNotNull(result);
        }


        [TestMethod]
        public async Task UpdateTask_ReturnSucces()
        {
            var action = new TaskActions(InvocationContext, FileManager);

            var input_1 = "New testing Title";
            var input_2 = "New testing Description";
            DateTime input_3 = DateTime.Now;
            var input_4 = new[] { 2, 6 };

            var input1 = new ProjectRequest { ProjectId = "750225" };
            var input2 = new UpdateTaskRequest { Title = input_1, Description = input_2, Deadline=input_3, FileIds= input_4 };
            string input3 = "4";

            var result = await action.UpdateTask(input1, input3, input2);

            var check = await action.GetTask(input1, input3);

            Assert.AreEqual(input_1, check.Title);
            Assert.IsNotNull(result);
        }


    }
}
