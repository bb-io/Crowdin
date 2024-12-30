using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.PreTranslations;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Crowdin.Api;

namespace Apps.Crowdin.Polling.Models.Requests;

public class PreTranslationStatusChangedRequest : PreTranslationRequest
{
    [Display("Statuses"), StaticDataSource(typeof(PreTranslationStatusDataSource))]
    public List<string> Statuses { get; set; } = new();

    public List<BuildStatus> GetCrowdinBuildStatuses()
    {
        var list = new List<BuildStatus>();
        Statuses.ForEach(status => list.Add(ToBuildStatus(status)));
        return list;
    }

    public static BuildStatus ToBuildStatus(string preTranslationStatus)
    {
        switch (preTranslationStatus)
        {
            case "created":
                return BuildStatus.Created;
            case "in_progress":
                return BuildStatus.InProgress;
                break;
            case "canceled":
                return BuildStatus.Canceled;
            case "failed":
                return BuildStatus.Failed;
            case "finished":
                return BuildStatus.Finished;
            default:
                throw new ArgumentException(
                    "Pre-translation status is wrong. Supported statuses: created, in_progress, canceled, failed, finished");
        }
    }
}