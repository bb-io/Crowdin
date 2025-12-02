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