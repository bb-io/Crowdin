using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Task;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.Task;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Crowdin.Api.Tasks;
using TaskStatus = Crowdin.Api.Tasks.TaskStatus;
using Apps.Crowdin.Models.Request.Users;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TaskActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AppInvocable(invocationContext)
{
    [Action("Search tasks", Description = "List all tasks")]
    public async Task<ListTasksResponse> ListTasks([ActionParameter] ListTasksRequest input)
    {
        var intProjectId = ParsingUtils.ParseOrThrow(input.ProjectId,nameof(input.ProjectId),id => 
        IntParser.Parse(id, nameof(input.ProjectId)));

        var intAssigneeId = ParsingUtils.ParseOrThrow(input.AssigneeId,nameof(input.AssigneeId),id => 
        IntParser.Parse(id, nameof(input.AssigneeId)));

        var status = ParsingUtils.ParseOrThrow(input.Status,nameof(input.Status),s => 
        EnumParser.Parse<TaskStatus>(s, nameof(input.Status)));

        var items = await Paginator.Paginate((lim, offset) =>
            ExceptionWrapper.ExecuteWithErrorHandling(() =>SdkClient.Tasks.ListTasks(intProjectId, lim, offset, status, intAssigneeId)));

        var tasks = items.Select(x => new TaskEntity(x)).ToArray();
        return new ListTasksResponse(tasks);
    }

    [Action("Get task", Description = "Get specific task")]
    public async Task<TaskEntity> GetTask(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Task ID")] string taskId)
    {
        var intProjectId = ParsingUtils.ParseOrThrow(project.ProjectId,nameof(project.ProjectId),id => 
        IntParser.Parse(id, nameof(project.ProjectId)));

        var intTaskId = ParsingUtils.ParseOrThrow(taskId,nameof(taskId),id => 
        IntParser.Parse(id, nameof(taskId)));

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(
            async () => await SdkClient.Tasks.GetTask(intProjectId, intTaskId));
        return new TaskEntity(response);
    }

    [Action("Add task", Description = "Add new task")]
    public async Task<TaskEntity> AddTask(
        [ActionParameter] AssigneesRequest project,
        [ActionParameter] AddNewTaskRequest input)
    {
        var vendorTaskTypes = new[] { "TranslateByVendor", "ProofreadByVendor" };
        if (vendorTaskTypes.Contains(input.Type) && input.Vendor is null)
        {
            throw new PluginMisconfigurationException("You should specify vendor for such task type");
        }

        if (input.Vendor is not null && !vendorTaskTypes.Contains(input.Type))
        {
            throw new PluginMisconfigurationException(
                "Task with the chosen type can't contain vendor. If you want to specify a vendor, please change type to 'Translate by vendor' or 'Proofread by vendor'");
        }

        var intProjectId = ParsingUtils.ParseOrThrow(project.ProjectId,nameof(project.ProjectId),id => IntParser.Parse(id, nameof(project.ProjectId)));

        var request = new CrowdinTaskCreateForm()
        {
            Title = input.Title,
            LanguageId = input.LanguageId,
            FileIds = input.FileIds.Select(fileId =>ParsingUtils.ParseOrThrow(fileId,nameof(fileId),id => 
            IntParser.Parse(id, nameof(fileId)))).ToList(),
            Type = ParsingUtils.ParseOrThrow(input.Type,nameof(input.Type),type => 
            EnumParser.Parse<TaskType>(type, nameof(input.Type))),
            Status = ParsingUtils.ParseOrThrow(input.Status,nameof(input.Status),status => 
            EnumParser.Parse<TaskStatus>(status, nameof(input.Status))),
            Description = input.Description,
            SplitFiles = input.SplitFiles,
            SkipAssignedStrings = input.SkipAssignedStrings,
            SkipUntranslatedStrings = input.SkipUntranslatedStrings,LabelIds = input.LabelIds?.Select(labelId =>
            ParsingUtils.ParseOrThrow(labelId,nameof(labelId),id => IntParser.Parse(id, nameof(labelId)))
           ).ToList(),
            Assignees = project.Assignees?.Select(assigneeId => new TaskAssigneeForm
           {
               Id = ParsingUtils.ParseOrThrow(assigneeId,nameof(assigneeId),id => IntParser.Parse(id, nameof(assigneeId)))
           }).ToList(),
            DeadLine = input.Deadline,
            DateFrom = input.DateFrom,
            DateTo = input.DateTo,
            IncludePreTranslatedStringsOnly = input.IncludePreTranslatedStringsOnly,
            Vendor = input.Vendor
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(
         async () => await SdkClient.Tasks.AddTask(intProjectId, request));
        return new(response);
    }

    [Action("Delete task", Description = "Delete specific task")]
    public async Task DeleteTask(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Task ID")] string taskId)
    {
        var intProjectId = ParsingUtils.ParseOrThrow(project.ProjectId,nameof(project.ProjectId),id => 
        IntParser.Parse(id, nameof(project.ProjectId)) );

        var intTaskId = ParsingUtils.ParseOrThrow(taskId,nameof(taskId),id => 
        IntParser.Parse(id, nameof(taskId)));

        await ExceptionWrapper.ExecuteWithErrorHandling( async () => 
        await SdkClient.Tasks.DeleteTask(intProjectId, intTaskId) );
    }

    [Action("Download task strings as XLIFF", Description = "Download specific task strings as XLIFF")]
    public async Task<DownloadFileResponse> DownloadTaskStrings(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Task ID")] string taskId)
    {
        var intProjectId = ParsingUtils.ParseOrThrow(project.ProjectId,nameof(project.ProjectId),id => 
            IntParser.Parse(id, nameof(project.ProjectId)));

        var intTaskId = ParsingUtils.ParseOrThrow(taskId,nameof(taskId),id => 
        IntParser.Parse(id, nameof(taskId)));

        var downloadLink = await ExceptionWrapper.ExecuteWithErrorHandling(
            async () => await SdkClient.Tasks.ExportTaskStrings(intProjectId, intTaskId));

        if (downloadLink is null)
            throw new PluginApplicationException("No string found for this task");

        var fileContent = await FileDownloader.DownloadFileBytes(downloadLink.Url);
        var file = await fileManagementClient.UploadAsync(fileContent.FileStream, fileContent.ContentType, fileContent.Name);
        return new DownloadFileResponse(file);
    }
}