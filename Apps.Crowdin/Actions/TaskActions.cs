using System.Net.Mime;
using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Task;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.Task;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Crowdin.Api.Tasks;
using File = Blackbird.Applications.Sdk.Common.Files.File;
using TaskStatus = Crowdin.Api.Tasks.TaskStatus;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TaskActions : BaseInvocable
{
    private AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    public TaskActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }
    
    [Action("List tasks", Description = "List all tasks")]
    public async Task<ListTasksResponse> ListTasks([ActionParameter] ListTasksRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intAssigneeId = IntParser.Parse(input.AssigneeId, nameof(input.AssigneeId));
        var status = EnumParser.Parse<TaskStatus>(input.Status, nameof(input.Status), EnumValues.TaskStatus);

        var client = new CrowdinClient(Creds);
        var items = await Paginator.Paginate((lim, offset)
            => client.Tasks.ListTasks(intProjectId!.Value, lim, offset, status, intAssigneeId));

        var tasks = items.Select(x => new TaskEntity(x)).ToArray();
        return new(tasks);
    }

    [Action("Get task", Description = "Get specific task")]
    public async Task<TaskEntity> GetTask(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Task ID")] string taskId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intTaskId = IntParser.Parse(taskId, nameof(taskId));

        var client = new CrowdinClient(Creds);

        var response = await client.Tasks.GetTask(intProjectId!.Value, intTaskId!.Value);
        return new(response);
    }

    [Action("Add task", Description = "Add new task")]
    public async Task<TaskEntity> AddTask(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] AddNewTaskRequest input)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));

        var client = new CrowdinClient(Creds);
        var request = new TaskCreateForm
        {
            Title = input.Title,
            LanguageId = input.LanguageId,
            FileIds = input.FileIds.Select(fileId => IntParser.Parse(fileId, nameof(fileId))!.Value).ToList(),
            Type = EnumParser.Parse<TaskType>(input.Type, nameof(input.Type), EnumValues.TaskType)!.Value,
            Status = EnumParser.Parse<TaskStatus>(input.Status, nameof(input.Status), EnumValues.TaskStatus),
            Description = input.Description,
            SplitFiles = input.SplitFiles,
            SkipAssignedStrings = input.SkipAssignedStrings,
            SkipUntranslatedStrings = input.SkipUntranslatedStrings,
            LabelIds = input.LabelIds?.Select(labelId => IntParser.Parse(labelId, nameof(labelId))!.Value).ToList(),
            Assignees = input.Assignees?.Select(assigneeId => new TaskAssigneeForm { Id = IntParser.Parse(assigneeId, nameof(assigneeId))!.Value }).ToList(),
            DeadLine = input.Deadline,
            DateFrom = input.DateFrom,
            DateTo = input.DateTo,
            IncludePreTranslatedStringsOnly = input.IncludePreTranslatedStringsOnly
        };
        
        var response = await client.Tasks.AddTask(intProjectId!.Value, request);
        return new(response);
    }

    [Action("Delete task", Description = "Delete specific task")]
    public Task DeleteTask(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Task ID")] string taskId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intTaskId = IntParser.Parse(taskId, nameof(taskId));

        var client = new CrowdinClient(Creds);

        return client.Tasks.DeleteTask(intProjectId!.Value, intTaskId!.Value);
    }

    [Action("Download task string", Description = "Download specific task strings")]
    public async Task<DownloadFileResponse> DownloadTaskStrings(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Task ID")] string taskId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intTaskId = IntParser.Parse(taskId, nameof(taskId));

        var client = new CrowdinClient(Creds);

        var downloadLink = await client.Tasks.ExportTaskStrings(intProjectId!.Value, intTaskId!.Value);

        if (downloadLink is null)
            throw new("No string found for this task");
        
        var fileContent = await FileDownloader.DownloadFileBytes(downloadLink.Url);

        var result = new File(fileContent)
        {
            ContentType = MediaTypeNames.Application.Octet,
        };
        return new(result);
    }
}