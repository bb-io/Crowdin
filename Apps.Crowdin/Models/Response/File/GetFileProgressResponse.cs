using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Response.File
{
    public class GetFileProgressResponse
    {
        [Display("Progress")]
        public IEnumerable<FileLanguageProgressEntity> Progress { get; set; }

        public GetFileProgressResponse(IEnumerable<FileLanguageProgressEntity> progress)
        {
            Progress = progress;
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
        public string LanguageId { get; set; }
        public string LanguageName { get; set; }
        public int TranslationProgress { get; set; }
        public int ApprovalProgress { get; set; }
        public int TotalWords { get; set; }
        public int TranslatedWords { get; set; }
        public int ApprovedWords { get; set; }
        public int TotalPhrases { get; set; }
        public int TranslatedPhrases { get; set; }
        public int ApprovedPhrases { get; set; }
    }

    public class StatsDto
    {
        [JsonProperty("total")]
        public int Total { get; set; }

        [JsonProperty("translated")]
        public int Translated { get; set; }

        [JsonProperty("preTranslateAppliedTo")]
        public int PreTranslateAppliedTo { get; set; }

        [JsonProperty("approved")]
        public int Approved { get; set; }
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
