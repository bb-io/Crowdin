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

        [Display("Statuses")]
        [StaticDataSource(typeof(TMExportStatusDataSource))]
        public List<string> Statuses { get; set; } = new();
    }
}
