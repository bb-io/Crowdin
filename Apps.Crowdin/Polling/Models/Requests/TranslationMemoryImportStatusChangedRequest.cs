using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Crowdin.Api;
using Crowdin.Api.TranslationMemory;

namespace Apps.Crowdin.Polling.Models.Requests
{
    public class TranslationMemoryImportStatusChangedRequest
    {
        [Display("Translation memory ID")]
        [DataSource(typeof(TmDataHandler))]
        public string TranslationMemoryId { get; set; } = default!;

        [Display("Import ID")]
        public string ImportId { get; set; } = default!;

        [Display("Statuses"), StaticDataSource(typeof(TMExportStatusDataSource))]
        public List<string> Statuses { get; set; } = new();

        public List<OperationStatus> GetCrowdinOperationStatuses()
        {
            var list = new List<OperationStatus>();
            Statuses.ForEach(status => list.Add(ToOperationStatus(status)));
            return list;
        }

        public static OperationStatus ToOperationStatus(string tmImportStatus)
        {
            switch (tmImportStatus)
            {
                case "created":
                    return OperationStatus.Created;
                case "in_progress":
                    return OperationStatus.InProgress;
                case "canceled":
                    return OperationStatus.Canceled;
                case "failed":
                    return OperationStatus.Failed;
                case "finished":
                    return OperationStatus.Finished;
                default:
                    throw new PluginMisconfigurationException(
                        "TM Import status is wrong. Supported statuses: created, in_progress, canceled, failed, finished");
            }
        }
    }
}
