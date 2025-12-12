using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Polling.Models;
using Apps.Crowdin.Polling.Models.Requests;
using Apps.Crowdin.Polling.Models.Responses;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Crowdin.Api;
using Crowdin.Api.Clients;
using Crowdin.Api.TranslationMemory;
using Crowdin.Api.Translations;
using RestSharp;
using System.Globalization;

namespace Apps.Crowdin.Polling;

[PollingEventList]
public class TMExportPollingList(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [PollingEvent("On TM export status changed", Description =
        "Triggered when the status of Translation Memory export changes to one of the specified statuses")]
    public async Task<PollingEventResponse<PollingMemory, TranslationMemoryStatusResponse>> OnPreTranslationStatusChanged(
        PollingEventRequest<PollingMemory> request,
        [PollingEventParameter] TranslationMemoryExportStatusChangedRequest tmExportStatusChangedRequest)
    {
        if (request.Memory is null)
        {
            return new()
            {
                FlyBird = false,
                Memory = new()
                {
                    LastPollingTime = DateTime.UtcNow,
                    Triggered = request.Memory?.Triggered ?? false
                }
            };
        }
        var tmExportStatusResponse = await SdkClient.TranslationMemory.CheckTmExportStatus(Convert.ToInt32(tmExportStatusChangedRequest.TranslationMemoryId), tmExportStatusChangedRequest.ExportId);

        var TmExportHasRightStatus = tmExportStatusChangedRequest.GetCrowdinOperationStatuses()
            .Any(x => x == tmExportStatusResponse.Status);

        var triggered = TmExportHasRightStatus && !request.Memory.Triggered;

        return new()
        {
            FlyBird = triggered,
            Result = new TranslationMemoryStatusResponse(tmExportStatusResponse),
            Memory = new()
            {
                LastPollingTime = DateTime.UtcNow,
                Triggered = triggered
            }
        };
    }

    [PollingEvent("On TM import status changed", Description = "Triggered when the status of Translation Memory import changes to one of the specified statuses")]
    public async Task<PollingEventResponse<PollingMemory, TranslationMemoryStatusResponse>> OnTmImportStatusChanged(
       PollingEventRequest<PollingMemory> request,
       [PollingEventParameter] TranslationMemoryImportStatusChangedRequest tmImportStatusChangedRequest)
    {
        var tmId = Convert.ToInt32(tmImportStatusChangedRequest.TranslationMemoryId);

        var importStatus = await GetTmImportStatusAsync(tmId, tmImportStatusChangedRequest.ImportId);

        var currentStatus = NormalizeStatus(importStatus.Status);

        var allowedStatuses = (tmImportStatusChangedRequest.Statuses ?? new())
            .Select(NormalizeStatus)
            .ToHashSet();

        var hasRightStatus = allowedStatuses.Contains(currentStatus);

        var previouslyTriggered = request.Memory?.Triggered ?? false;
        var triggeredNow = hasRightStatus && !previouslyTriggered;

        return new()
        {
            FlyBird = triggeredNow,
            Result = triggeredNow
                ? new TranslationMemoryStatusResponse(importStatus)
                : null,
            Memory = new()
            {
                LastPollingTime = DateTime.UtcNow,
                Triggered = triggeredNow || previouslyTriggered
            }
        };
    }

    [PollingEvent("All tasks have reached a status",
        Description = "Triggered when all matching tasks are in one of the specified statuses (default: done).")]
    public async Task<PollingEventResponse<TasksPollingMemory, AllTasksReachedStatusResponse>> OnAllTasksReachedStatus(
        PollingEventRequest<TasksPollingMemory> request,
        [PollingEventParameter] AllTasksReachedStatusRequest input)
    {
        if (string.IsNullOrWhiteSpace(input.ProjectId))
            throw new PluginMisconfigurationException("Project ID is required.");

        var isFirstRun = request.Memory is null;

        var memory = request.Memory ?? new TasksPollingMemory
        {
            LastPollingTime = DateTime.UtcNow,
            Triggered = false
        };

        var desiredStatuses = (input.Status?.Where(x => !string.IsNullOrWhiteSpace(x)).ToArray()
                               ?? new[] { "done" })
            .Select(x => x.Trim())
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var now = DateTime.UtcNow;

        if (!isFirstRun)
        {
            var updatedSinceLastPoll = await ListTasksUpdatedSinceAsync(
                input,
                memory.LastPollingTime,
                limit: 50);

            if (updatedSinceLastPoll.Count == 0)
            {
                return new()
                {
                    FlyBird = false,
                    Memory = new TasksPollingMemory
                    {
                        LastPollingTime = now,
                        Triggered = memory.Triggered
                    }
                };
            }
        }

        var allMatchingTasks = await ListAllMatchingTasksAsync(input, limit: 50);

        if (!string.IsNullOrWhiteSpace(input.TitleContains))
        {
            var needle = input.TitleContains.Trim();
            allMatchingTasks = allMatchingTasks
                .Where(t => (t.Title ?? "").Contains(needle, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        var allReady = allMatchingTasks.Any() &&
                       allMatchingTasks.All(t => t.Status != null && desiredStatuses.Contains(t.Status));

        var triggered = allReady && !memory.Triggered;

        return new()
        {
            FlyBird = triggered,
            Result = triggered
                ? new AllTasksReachedStatusResponse
                {
                    Tasks = allMatchingTasks,
                    TaskIds = allMatchingTasks.Select(t => t.Id.ToString(CultureInfo.InvariantCulture)).ToList()
                }
                : null,
            Memory = new TasksPollingMemory
            {
                LastPollingTime = now,
                Triggered = triggered || memory.Triggered
            }
        };
    }

    private async Task<List<TaskResource>> ListTasksUpdatedSinceAsync(
          AllTasksReachedStatusRequest input,
          DateTimeOffset since,
          int limit)
    {
        var result = new List<TaskResource>();
        var offset = 0;

        while (true)
        {
            var page = await ListTasksPageAsync(input, limit, offset);

            if (page.Count == 0)
                break;

            foreach (var task in page)
            {
                var updatedAt = task.UpdatedAt ?? DateTimeOffset.MinValue;

                if (updatedAt > since)
                    result.Add(task);
                else
                    return result;
            }

            offset += limit;
        }

        return result;
    }

    private async Task<List<TaskResource>> ListAllMatchingTasksAsync(AllTasksReachedStatusRequest input, int limit)
    {
        var result = new List<TaskResource>();
        var offset = 0;

        while (true)
        {
            var page = await ListTasksPageAsync(input, limit, offset);
            if (page.Count == 0)
                break;

            result.AddRange(page);
            offset += limit;
        }

        return result;
    }

    private async Task<List<TaskResource>> ListTasksPageAsync(AllTasksReachedStatusRequest input, int limit, int offset)
    {
        if (string.IsNullOrWhiteSpace(input.ProjectId))
            throw new PluginMisconfigurationException("Project ID is required.");

        var plan = InvocationContext.AuthenticationCredentialsProviders.GetCrowdinPlan();
        BlackBirdRestClient restClient = plan == Plans.Enterprise
            ? new CrowdinEnterpriseRestClient(InvocationContext.AuthenticationCredentialsProviders)
            : new CrowdinRestClient();

        var req = new CrowdinRestRequest(
            $"/projects/{input.ProjectId}/tasks",
            Method.Get,
            InvocationContext.AuthenticationCredentialsProviders);

        req.AddQueryParameter("limit", limit);
        req.AddQueryParameter("offset", offset);

        req.AddQueryParameter("orderBy", "updatedAt desc");

        if (input.Status != null)
        {
            foreach (var s in input.Status.Where(x => !string.IsNullOrWhiteSpace(x)))
                req.AddQueryParameter("status", s.Trim());
        }

        var response = await restClient.ExecuteWithErrorHandling<ListResponse<TaskResource>>(req);
        return response.Data.Select(x => x.Data).ToList();
    }


    private static string NormalizeStatus(string status)
    {
        return status
            .Trim()
            .Replace("_", "", StringComparison.OrdinalIgnoreCase)
            .ToLowerInvariant();
    }

    private async Task<TmImportStatusApiModel> GetTmImportStatusAsync(int tmId, string importId)
    {
        var plan = InvocationContext.AuthenticationCredentialsProviders.GetCrowdinPlan();
        BlackBirdRestClient restClient = plan == Plans.Enterprise
           ? new CrowdinEnterpriseRestClient(invocationContext.AuthenticationCredentialsProviders)
           : new CrowdinRestClient();

        var request = new CrowdinRestRequest($"/tms/{tmId}/imports/{importId}", Method.Get, InvocationContext.AuthenticationCredentialsProviders);

        var response = await restClient.ExecuteWithErrorHandling<TmImportStatusApiEnvelope>(request);
        if (response?.Data == null)
            throw new PluginApplicationException("Empty response from Crowdin when checking TM import status");

        return response.Data;
    }
}