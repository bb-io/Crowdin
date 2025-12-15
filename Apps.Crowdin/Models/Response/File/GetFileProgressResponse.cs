using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Response.File
{
    public class GetFileProgressResponse
    {
        [Display("Progress")]
        public IEnumerable<FileLanguageProgressEntity> Progress { get; set; }

        [Display("All languages translated")]
        public bool AllTranslated {get; set;}

        [Display("All languages approved")]
        public bool AllApproved { get; set; }

        public GetFileProgressResponse(IEnumerable<FileLanguageProgressEntity> progress)
        {
            Progress = progress;
            AllTranslated = progress.All(x => x.TranslationProgress == 100);
            AllApproved = progress.All(x => x.ApprovalProgress == 100);
        }
    }

    public class LanguageProgressResponseDto
    {
        [JsonProperty("data")]
        public List<LanguageProgressDataWrapperDto> Data { get; set; }

        [JsonProperty("pagination")]
        public PaginationDto Pagination { get; set; }
    }

    public class LanguageProgressDataWrapperDto
    {
        [JsonProperty("data")]
        public LanguageProgressDto Data { get; set; }
    }
    public class FileLanguageProgressEntity
    {
        [Display("Language ID")]
        public string LanguageId { get; set; }

        [Display("Language name")]
        public string LanguageName { get; set; }

        [Display("Translation progress")]
        public int TranslationProgress { get; set; }

        [Display("Approval progress")]
        public int ApprovalProgress { get; set; }

        [Display("Total words")]
        public int TotalWords { get; set; }

        [Display("Translated words")]
        public int TranslatedWords { get; set; }

        [Display("Approved words")]
        public int ApprovedWords { get; set; }

        [Display("Total phrases")]
        public int TotalPhrases { get; set; }

        [Display("Translated phrases")]
        public int TranslatedPhrases { get; set; }

        [Display("Approved phrases")]
        public int ApprovedPhrases { get; set; }
    }

    public class StatsDto
    {
        [JsonProperty("total")]
        public int? Total { get; set; }

        [JsonProperty("translated")]
        public int? Translated { get; set; }

        [JsonProperty("preTranslateAppliedTo")]
        public int? PreTranslateAppliedTo { get; set; }

        [JsonProperty("approved")]
        public int? Approved { get; set; }
    }
    public class LanguageDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }

    public class LanguageProgressDto
    {
        [JsonProperty("languageId")]
        public string LanguageId { get; set; }

        [JsonProperty("eTag")]
        public string ETag { get; set; }

        [JsonProperty("language")]
        public LanguageDto Language { get; set; }

        [JsonProperty("words")]
        public StatsDto Words { get; set; }

        [JsonProperty("phrases")]
        public StatsDto Phrases { get; set; }

        [JsonProperty("translationProgress")]
        public int TranslationProgress { get; set; }

        [JsonProperty("approvalProgress")]
        public int ApprovalProgress { get; set; }
    }
}
