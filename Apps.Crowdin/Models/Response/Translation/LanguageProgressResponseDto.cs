using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.Models.Dtos;
using Apps.Crowdin.Models.Response.File;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Response.Translation
{
    public class LanguageProgressResponseDto
    {
        [JsonProperty("data")]
        [Display("Data")]
        public IEnumerable<LanguageProgressDataWrapperDto> Data { get; set; }

    }

    public class LanguageProgressDataWrapperDto
    {
        [JsonProperty("data")]
        public LanguageProgressDto Data { get; set; }
    }

    public class LanguageProgressDto
    {
        [JsonProperty("words")]
        [Display("Words")]
        public StatsDto Words { get; set; }

        [JsonProperty("phrases")]
        [Display("Phrases")]
        public StatsDto Phrases { get; set; }

        [JsonProperty("translationProgress")]
        [Display("Translation progress")]
        public int TranslationProgress { get; set; }

        [JsonProperty("approvalProgress")]
        [Display("Approval progress")]
        public int ApprovalProgress { get; set; }

        [JsonProperty("fileId")]
        [Display("File ID")]
        public int FileId { get; set; }

        [JsonProperty("eTag")]
        [Display("ETag")]
        public string ETag { get; set; }
    }

    public class StatsDto
    {
        [JsonProperty("total")]
        [Display("Total")]
        public int Total { get; set; }

        [JsonProperty("translated")]
        [Display("Translated")]
        public int Translated { get; set; }

        [JsonProperty("preTranslateAppliedTo")]
        [Display("PreTranslate applied to")]
        public int PreTranslateAppliedTo { get; set; }

        [JsonProperty("approved")]
        [Display("Approved")]
        public int Approved { get; set; }
    }

    public class SimplifiedLanguageProgressResponseDto
    {
        [JsonProperty("data")]
        [Display("Data")]
        public IEnumerable<SimplifiedLanguageProgressDto> Data { get; set; }
    }

    public class SimplifiedLanguageProgressDto
    {
        [JsonProperty("words")]
        [Display("Words")]
        public StatsDto Words { get; set; }

        [JsonProperty("phrases")]
        [Display("Phrases")]
        public StatsDto Phrases { get; set; }

        [JsonProperty("translationProgress")]
        [Display("Translation progress")]
        public int TranslationProgress { get; set; }

        [JsonProperty("approvalProgress")]
        [Display("Approval progress")]
        public int ApprovalProgress { get; set; }

        [JsonProperty("fileId")]
        [Display("File ID")]
        public int FileId { get; set; }

        [JsonProperty("eTag")]
        [Display("ETag")]
        public string ETag { get; set; }
    }

}
