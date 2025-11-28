using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.TranslationMemory
{
    public class SearchTmSegmentsRequest
    {
        [Display("Language ID")]
        [DataSource(typeof(LanguagesDataHandler))]
        public string? LanguageId { get; set; }
    }
}
