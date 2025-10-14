using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Extensions;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Dtos;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Task;
using Apps.Crowdin.Models.Request.Users;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.Task;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Crowdin.Api.ProjectsGroups;
using Crowdin.Api.Tasks;
using RestSharp;
using TaskStatus = Crowdin.Api.Tasks.TaskStatus;

namespace Apps.Crowdin.Actions;

[ActionList("Tasks")]
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

    [Action("[Enterprise] Add vendor task", Description = "Add new vendor task for a specific workflow")]
    public async Task<TaskEntity> AddVendorWorkflowTask(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] AddNewVendorTaskRequest input)
    {
        CheckAccessToEnterpriseAction();
        ValidateDeadline(input.Deadline);

        int projectId = (int)IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!;

        if (input.FileIds.Any(f => !int.TryParse(f, out _)))
        {
            var invalid = input.FileIds.First(f => !int.TryParse(f, out _));
            throw new PluginMisconfigurationException(
                $"Invalid File ID: {invalid} must be a numeric value. Please check the input file ID"
            );
        }

        await ValidateTargetLanguage(projectId, input.LanguageId);

        var client = new CrowdinEnterpriseRestClient(Creds);
        var request = new CrowdinRestRequest($"projects/{projectId}/tasks", Method.Post, Creds);

        var body = new Dictionary<string, object>
        {
            ["workflowStepId"] = IntParser.Parse(input.WorkflowStepId, nameof(input.WorkflowStepId))!,
            ["title"] = input.Title,
            ["languageId"] = input.LanguageId,
            ["fileIds"] = input.FileIds.Select(int.Parse).ToList(),
        };

        body.AddIfNotNullOrEmpty("description", input.Description);
        body.AddIfNotNullOrEmpty("status", input.Status);
        body.AddIfHasValue("deadline", input.Deadline);
        body.AddIfHasValue("dateFrom", input.DateFrom);
        body.AddIfHasValue("dateTo", input.DateTo);

        request.AddBody(body);

        var response = await client.ExecuteWithErrorHandling<DataResponse<TaskResourceDto>>(request);
        return new TaskEntity(response.Data);
    }

    [Action("Add task", Description = "Add new task")]
    public async Task<TaskEntity> AddTask(
        [ActionParameter] AssigneesRequest project,
        [ActionParameter] AddNewTaskRequest input)
    {
        ValidateDeadline(input.Deadline);
        int projectId = (int)IntParser.Parse(project.ProjectId, nameof(project.ProjectId))!;

        await ValidateTargetLanguage(projectId, input.LanguageId);

        var request = new TaskCreateForm()
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
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Tasks.AddTask(projectId, request)
        );
        return new(response);
    }

    [Action("Add pending task", Description = "Add new pending task")]
    public async Task<TaskEntity> AddPendingTask(
    [ActionParameter] AssigneesRequest project,
    [ActionParameter] AddNewPendingTaskRequest input)
    {
        
        if (!int.TryParse(project.ProjectId, out var intProjectId))
            throw new PluginMisconfigurationException($"Invalid Project ID: {project.ProjectId} must be a numeric value. Please check the input project ID");

        if (!int.TryParse(input.PrecedingTask, out var intPrecedingTask))
            throw new PluginMisconfigurationException($"Invalid Preceding task ID: {input.PrecedingTask} must be a numeric value. Please check the input task ID");


        var request = new PendingTaskCreateForm()
        {
            PrecedingTaskId = intPrecedingTask,
            Title = input.Title,
            Type = EnumParser.Parse<TaskType>(input.Type, nameof(input.Type))!.Value,
            Description = input.Description,
            Assignees = project.Assignees?.Select(assigneeId =>
            {
                if (!int.TryParse(assigneeId, out var parsedAssigneeId))
                    throw new PluginMisconfigurationException($"Invalid Assignee ID: {assigneeId} must be a numeric value. Please check the input assignee ID");
                return new TaskAssigneeForm { Id = parsedAssigneeId };
            }).ToList(),
            DeadLine = input.Deadline
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
        var taskEntity = await GetTask(project, taskId);
        if (taskEntity == null)
            throw new PluginApplicationException($"Task with ID {taskId} does not exist. Please check the input");

        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intTaskId = IntParser.Parse(taskId, nameof(taskId));

        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Tasks.DeleteTask(intProjectId!.Value, intTaskId!.Value));
    }

    [Action("Update task", Description = "Partially update specific task via JSON Patch (RFC 6902)")]
    public async Task<TaskEntity> UpdateTask(
         [ActionParameter] ProjectRequest project,
         [ActionParameter][Display("Task ID")] string taskId,
         [ActionParameter] UpdateTaskRequest input)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intTaskId = IntParser.Parse(taskId, nameof(taskId));

        var patchOperations = new List<TaskPatchOperation>();

        if (input.Status is not null)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/status",
                Value = input.Status
            });
        }
        if (input.Title is not null)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/title",
                Value = input.Title
            });
        }
        if (input.Description is not null)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/description",
                Value = input.Description
            });
        }
        if (input.Deadline.HasValue)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/deadline",
                Value = input.Deadline.Value.ToString("O")
            });
        }
        if (input.StartedAt.HasValue)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/startedAt",
                Value = input.StartedAt.Value.ToString("O")
            });
        }
        if (input.ResolvedAt.HasValue)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/resolvedAt",
                Value = input.ResolvedAt.Value.ToString("O")
            });
        }
        if (input.FileIds is not null)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/fileIds",
                Value = input.FileIds.Select(id => IntParser.Parse(id, nameof(input.FileIds))).ToArray()
            });
        }
        if (input.StringIds is not null)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/stringIds",
                Value = input.StringIds.Select(id => IntParser.Parse(id, nameof(input.StringIds))).ToArray()
            });
        }
        if (input.DateFrom.HasValue)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/dateFrom",
                Value = input.DateFrom.Value.ToString("O")
            });
        }
        if (input.DateTo.HasValue)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/dateTo",
                Value = input.DateTo.Value.ToString("O")
            });
        }
        if (input.LabelIds is not null)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/labelIds",
                Value = input.LabelIds.Select(id => IntParser.Parse(id, nameof(input.LabelIds))).ToArray()
            });
        }
        if (input.ExcludeLabelIds is not null)
        {
            patchOperations.Add(new TaskPatchOperation
            {
                Op = "replace",
                Path = "/excludeLabelIds",
                Value = input.ExcludeLabelIds.Select(id => IntParser.Parse(id, nameof(input.ExcludeLabelIds))).ToArray()
            });
        }

        if (patchOperations.Count == 0)
        {
            throw new PluginMisconfigurationException("No update fields provided. Please specify at least one field to update.");
        }

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Tasks.EditTask(intProjectId!.Value, intTaskId!.Value, patchOperations.Cast<TaskPatchBase>().ToList()));

        return new TaskEntity(response);
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
            throw new PluginApplicationException($"Task with ID {taskId} does not exist. Please check the input");

        var downloadLink = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
        await SdkClient.Tasks.ExportTaskStrings(intProjectId, intTaskId));

        if (downloadLink is null)
            throw new PluginApplicationException("No string found for this task");

        var fileContent = await FileDownloader.DownloadFileBytes(downloadLink.Url);
        await FileOperationWrapper.ExecuteFileOperation(() => Task.CompletedTask, fileContent.FileStream, fileContent.Name);
        var file = await fileManagementClient.UploadAsync(fileContent.FileStream, fileContent.ContentType,
            fileContent.Name);
        return new(file);
    }

    public static void ValidateDeadline(DateTime? deadline)
    {
        if (deadline.HasValue && deadline.Value <= DateTime.UtcNow)
            throw new PluginMisconfigurationException("Deadline must be in the future");
    }

    public async Task ValidateTargetLanguage(int projectId, string languageId)
    {
        var projectInfo = await ExceptionWrapper.ExecuteWithErrorHandling(() =>
            SdkClient.ProjectsGroups.GetProject<ProjectBase>(projectId)
        );

        if (projectInfo?.TargetLanguageIds == null || !projectInfo.TargetLanguageIds.Contains(languageId))
            throw new PluginMisconfigurationException(
                "The input language is not set in the target project. Please check the supported target languages of your project"
            );
    }
}