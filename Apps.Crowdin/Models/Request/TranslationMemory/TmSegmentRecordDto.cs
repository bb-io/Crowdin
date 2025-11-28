using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Request.TranslationMemory
{
    public class TmSegmentRecordDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("languageId")]
        public string? LanguageId { get; set; }

        [JsonProperty("text")]
        public string? Text { get; set; }
    }

    public class TmSegmentDataDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("records")]
        public List<TmSegmentRecordDto> Records { get; set; } = new();
    }

    public class TmSegmentResponseDto
    {
        [JsonProperty("data")]
        public TmSegmentDataDto Data { get; set; } = default!;
    }
}
