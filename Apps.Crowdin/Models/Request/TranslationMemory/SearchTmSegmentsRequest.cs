using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.TranslationMemory
{
    public class SearchTmSegmentsRequest
    {
        [Display("Language ID")]
        public string? LanguageId { get; set; }
    }
}
