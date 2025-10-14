using Newtonsoft.Json;
using Tests.Crowdin.Base;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.Task;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Users;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Tests.Crowdin;

[TestClass]
public class TaskTests : TestBase
{
    [TestMethod]
    public async Task AddVendorWorkflowTask_ReturnsSuccess()
    {
        // Arrange
        var action = new TaskActions(InvocationContext, FileManager);
        var project = new ProjectRequest { ProjectId = "1" };
        var input = new AddNewVendorTaskRequest
        {
            WorkflowStepId = "7",
            Title = "Hello from tests3111",
            LanguageId = "uk",
            FileIds = ["1"],
            Status = "in_progress",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };

        // Act
        var result = await action.AddVendorWorkflowTask(project, input);

        // Assert
        Assert.IsNotNull(result);
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }

    [TestMethod]
    public async Task AddVendorWorkflowTask_IncorrectTargetLanguage_ThrowsMisconfigException()
    {
        // Arrange
        var action = new TaskActions(InvocationContext, FileManager);
        var project = new ProjectRequest { ProjectId = "1" };
        var input = new AddNewVendorTaskRequest
        {
            WorkflowStepId = "7",
            Title = "Hello from tests3111",
            LanguageId = "es",
            FileIds = ["1"],
            Status = "in_progress",
            Deadline = DateTime.Now + TimeSpan.FromDays(2)
        };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(async () => await action.AddVendorWorkflowTask(project, input));
    }

    [TestMethod]
    public async Task AddTask_ReturnsSuccess()
    {
        // Arrange
        var action = new TaskActions(InvocationContext, FileManager);
        var project = new AssigneesRequest 
        { 
            ProjectId = "1", 
            Assignees = ["9"] 
        };
        var input = new AddNewTaskRequest
        {
            Title = "Hello from tests3111",
            LanguageId = "uk",
            FileIds = ["1"],
            Status = "Todo",
            Type = "Proofread",
            Deadline = DateTime.UtcNow + TimeSpan.FromDays(3),
        };

        // Act
        var result = await action.AddTask(project, input);

        // Assert
        Assert.IsNotNull(result);
        Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));
    }

    [TestMethod]
    public async Task AddTask_IncorrectDeadline_ThrowsMisconfigException()
    {
        // Arrange
        var action = new TaskActions(InvocationContext, FileManager);
        var project = new AssigneesRequest
        {
            ProjectId = "1",
            Assignees = ["9"]
        };
        var input = new AddNewTaskRequest
        {
            Title = "Hello from tests3111",
            LanguageId = "uk",
            FileIds = ["1"],
            Status = "Todo",
            Type = "Proofread",
            Deadline = DateTime.UtcNow - TimeSpan.FromDays(3),
        };

        // Act & Assert
        await Assert.ThrowsExceptionAsync<PluginMisconfigurationException>(async () => await action.AddTask(project, input));
    }

    [TestMethod]
    public async Task ListTask_ReturnsSuccess()
    {
        var action = new TaskActions(InvocationContext, FileManager);

        var input1 = new ListTasksRequest { ProjectId = "1" };

        var result = await action.ListTasks(input1);

        foreach (var task in result.Tasks)
        {
            Console.WriteLine($"{task.Id} - {task.Title} - {string.Join(", ", task.Assignees)}");
            Assert.IsNotNull(result);
        }
    }

    [TestMethod]
    public async Task GetTask_ReturnsSuccess()
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
    public async Task UpdateTask_ReturnsSuccess()
    {
        var action = new TaskActions(InvocationContext, FileManager);

        var input_1 = "New testing Title";
        var input_2 = "New testing Description";
        DateTime input_3 = DateTime.Now;
        var input_4 = new[] { "2", "6" };

        var input1 = new ProjectRequest { ProjectId = "750225" };
        var input2 = new UpdateTaskRequest { Title = input_1, Description = input_2, Deadline=input_3, FileIds= input_4 };
        string input3 = "4";

        var result = await action.UpdateTask(input1, input3, input2);

        var check = await action.GetTask(input1, input3);

        Assert.AreEqual(input_1, check.Title);
        Assert.IsNotNull(result);
    }
}
