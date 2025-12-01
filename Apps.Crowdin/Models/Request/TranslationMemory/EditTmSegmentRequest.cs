using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.TranslationMemory
{
    public class EditTmSegmentRequest
    {
        [Display("Operation")]
        [StaticDataSource(typeof(TmSegmentOperationDataSourceHandler))]
        public string Operation { get; set; } = default!;

        [Display("Record ID")]
        public string? RecordId { get; set; }

        [Display("Text")]
        public string? Text { get; set; }

        [Display("Language ID")]
        [DataSource(typeof(LanguagesDataHandler))]
        public string? LanguageId { get; set; }
    }
}
