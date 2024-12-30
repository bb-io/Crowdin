using Apps.Crowdin.Api;
using Apps.Crowdin.Polling.Models;
using Apps.Crowdin.Polling.Models.Requests;
using Apps.Crowdin.Polling.Models.Responses;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.Crowdin.Polling;

[PollingEventList]
public class PreTranslationPollingList(InvocationContext invocationContext) : BaseInvocable(invocationContext)
{
    [PollingEvent("On pre-translation status changed",
        "Triggered when the status of a pre-translation changes to one of the specified statuses.")]
    public async Task<PollingEventResponse<PollingMemory, PreTranslationResponse>> OnPreTranslationStatusChanged(
        PollingEventRequest<PollingMemory> request,
        [PollingEventParameter] PreTranslationStatusChangedRequest preTranslationStatusChangedRequest)
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

        var client = new CrowdinClient(InvocationContext.AuthenticationCredentialsProviders);
        var preTranslation = await client.Translations.GetPreTranslationStatus(
            int.Parse(preTranslationStatusChangedRequest.ProjectId),
            preTranslationStatusChangedRequest.PreTranslationId);

        var hasRightStatus = preTranslationStatusChangedRequest.GetCrowdinBuildStatuses()
            .Any(x => preTranslation.Status == x);
        var triggered = hasRightStatus && !request.Memory.Triggered;
        return new()
        {
            FlyBird = triggered,
            Result = new(preTranslation),
            Memory = new()
            {
                LastPollingTime = DateTime.UtcNow,
                Triggered = triggered
            }
        };
    }
}