using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Crowdin.Api;

namespace Apps.Crowdin.Polling.Models.Requests;

public class PreTranslationStatusChangedRequest : ProjectRequest
{
    [Display("Pre-translation IDs",
         Description = "If you specify this property, the event will not check all pre-translations within the specific project, but will only check the specified pre-translations."),
     DataSource(typeof(PreTranslationDataSource))]
    public IEnumerable<string>? PreTranslationIds { get; set; } = default!;

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