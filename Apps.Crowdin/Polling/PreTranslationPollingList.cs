using Apps.Crowdin.Api;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Polling.Models;
using Apps.Crowdin.Polling.Models.Requests;
using Apps.Crowdin.Polling.Models.Responses;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Polling;

namespace Apps.Crowdin.Polling;

[PollingEventList]
public class PreTranslationPollingList(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [PollingEvent("On pre-translations status changed", Description = 
        "Triggered when the status of all pre-translations in a project changes to one of the specified statuses")]
    public async Task<PollingEventResponse<PollingMemory, PreTranslationsResponse>> OnPreTranslationStatusChanged(
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

        var preTranslations =
            await Paginator.Paginate((lim, offset)
                => SdkClient.Translations.ListPreTranslations(int.Parse(preTranslationStatusChangedRequest.ProjectId), lim, offset));

        if (preTranslationStatusChangedRequest.PreTranslationIds != null)
        {
            preTranslations = preTranslations
                .Where(x => preTranslationStatusChangedRequest.PreTranslationIds.Contains(x.Identifier))
                .ToList();
        }

        var allPreTranslationsHasRightStatus = preTranslations.All(y => preTranslationStatusChangedRequest.GetCrowdinBuildStatuses()
            .Any(x => y.Status == x));
        var triggered = allPreTranslationsHasRightStatus && !request.Memory.Triggered;
        return new()
        {
            FlyBird = triggered,
            Result = new(preTranslations.Select(x => new PreTranslationResponse(x)).ToList()),
            Memory = new()
            {
                LastPollingTime = DateTime.UtcNow,
                Triggered = triggered
            }
        };
    }
}