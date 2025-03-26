using Apps.Crowdin.Invocables;
using Apps.Crowdin.Polling.Models;
using Apps.Crowdin.Polling.Models.Requests;
using Apps.Crowdin.Polling.Models.Responses;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Crowdin.Api;
using Crowdin.Api.Translations;

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
}