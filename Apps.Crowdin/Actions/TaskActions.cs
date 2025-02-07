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
using Crowdin.Api.ProjectsGroups;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TaskActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AppInvocable(invocationContext)
{
    [Action("Search tasks", Description = "List all tasks")]
    public async Task<ListTasksResponse> ListTasks([ActionParameter] ListTasksRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intAssigneeId = IntParser.Parse(input.AssigneeId, nameof(input.AssigneeId));
        var status = EnumParser.Parse<TaskStatus>(input.Status, nameof(input.Status));

        var items = await Paginator.Paginate((lim, offset)
            => ExceptionWrapper.ExecuteWithErrorHandling(() =>
                SdkClient.Tasks.ListTasks(intProjectId!.Value, lim, offset, status, intAssigneeId)));

        var tasks = items.Select(x => new TaskEntity(x)).ToArray();
        return new(tasks);
    }

    [Action("Get task", Description = "Get specific task")]
    public async Task<TaskEntity> GetTask(
        [ActionParameter] ProjectRequest project,
        [ActionParameter][Display("Task ID")] string taskId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intTaskId = IntParser.Parse(taskId, nameof(taskId));

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Tasks.GetTask(intProjectId!.Value, intTaskId!.Value));
        return new(response);
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

        if (!int.TryParse(project.ProjectId, out var intProjectId))
            throw new PluginMisconfigurationException($"Invalid Project ID: {project.ProjectId} must be a numeric value. Please check the input project ID");

        var projectInfo = await ExceptionWrapper.ExecuteWithErrorHandling(() =>
        SdkClient.ProjectsGroups.GetProject<ProjectBase>(intProjectId));

        if (projectInfo?.TargetLanguageIds == null
        || !projectInfo.TargetLanguageIds.Contains(input.LanguageId))
        {
            throw new PluginMisconfigurationException(
                $"The input language is not setted in the target project. Please check the supported target languages by your project" );
        }

        var request = new CrowdinTaskCreateForm()
        {
            Title = input.Title,
            LanguageId = input.LanguageId,
            FileIds = input.FileIds.Select(fileId =>
            {
                if (!int.TryParse(fileId, out var parsedFileId))
                    throw new PluginMisconfigurationException($"Invalid File ID: {fileId} must be a numeric value. Please check the input file ID");
                return parsedFileId;
            }).ToList(),
            Type = EnumParser.Parse<TaskType>(input.Type, nameof(input.Type))!.Value,
            Status = EnumParser.Parse<TaskStatus>(input.Status, nameof(input.Status)),
            Description = input.Description,
            SplitFiles = input.SplitFiles,
            SkipAssignedStrings = input.SkipAssignedStrings,
            SkipUntranslatedStrings = input.SkipUntranslatedStrings,
            LabelIds = input.LabelIds?.Select(labelId =>
            {
                if (!int.TryParse(labelId, out var parsedLabelId))
                    throw new PluginMisconfigurationException($"Invalid Label ID: {labelId} must be a numeric value. Please check the input label ID");
                return parsedLabelId;
            }).ToList(),
            Assignees = project.Assignees?.Select(assigneeId =>
            {
                if (!int.TryParse(assigneeId, out var parsedAssigneeId))
                    throw new PluginMisconfigurationException($"Invalid Assignee ID: {assigneeId} must be a numeric value. Please check the input assignee ID");
                return new TaskAssigneeForm { Id = parsedAssigneeId };
            }).ToList(),
            DeadLine = input.Deadline,
            DateFrom = input.DateFrom,
            DateTo = input.DateTo,
            IncludePreTranslatedStringsOnly = input.IncludePreTranslatedStringsOnly,
            Vendor = input.Vendor
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
        await SdkClient.Tasks.AddTask(intProjectId, request));
        return new(response);
    }

    [Action("Delete task", Description = "Delete specific task")]
    public async Task DeleteTask(
        [ActionParameter] ProjectRequest project,
        [ActionParameter][Display("Task ID")] string taskId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intTaskId = IntParser.Parse(taskId, nameof(taskId));

        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Tasks.DeleteTask(intProjectId!.Value, intTaskId!.Value));
    }

    [Action("Download task strings as XLIFF", Description = "Download specific task strings as XLIFF")]
    public async Task<DownloadFileResponse> DownloadTaskStrings(
        [ActionParameter] ProjectRequest project,
        [ActionParameter][Display("Task ID")] string taskId)
    {
        if (!int.TryParse(project.ProjectId, out var intProjectId))
            throw new PluginMisconfigurationException(
                $"Invalid Project ID: {project.ProjectId} must be a numeric value. Please check the input project ID");

        if (!int.TryParse(taskId, out var intTaskId))
            throw new PluginMisconfigurationException(
                $"Invalid Task ID: {taskId} must be a numeric value. Please check the input task ID");

        var taskEntity = await GetTask(project, taskId);
        if (taskEntity == null)
            throw new PluginApplicationException($"Task with ID {taskId} does not exist.");

        var downloadLink = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
        await SdkClient.Tasks.ExportTaskStrings(intProjectId, intTaskId));

        if (downloadLink is null)
            throw new PluginApplicationException("No string found for this task");

        var fileContent = await FileDownloader.DownloadFileBytes(downloadLink.Url);
        var file = await fileManagementClient.UploadAsync(fileContent.FileStream, fileContent.ContentType,
            fileContent.Name);
        return new(file);
    }
}