using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Crowdin.Api;

namespace Apps.Crowdin.Polling.Models.Requests
{
    public class TranslationMemoryExportStatusChangedRequest
    {
        [Display("Translation memory ID")]
        [DataSource(typeof(TmDataHandler))]
        public string TranslationMemoryId { get; set; }

        [Display("Export ID")]
        [DataSource(typeof(TmDataHandler))]
        public string ExportId { get; set; }
    }
}