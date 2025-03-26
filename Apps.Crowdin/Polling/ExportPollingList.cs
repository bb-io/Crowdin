using Apps.Crowdin.Invocables;
using Apps.Crowdin.Polling.Models;
using Apps.Crowdin.Polling.Models.Requests;
using Apps.Crowdin.Polling.Models.Responses;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;
using Crowdin.Api;

namespace Apps.Crowdin.Polling;

[PollingEventList]
public class ExportPollingList(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [PollingEvent("On TM Export Status Changed", Description = 
        "")] //TODO: fill out description
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

        var TmExportHasRightStatus = false;

        if (tmExportStatusResponse.Status == OperationStatus.Finished)
        {
            TmExportHasRightStatus = true;
        }

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