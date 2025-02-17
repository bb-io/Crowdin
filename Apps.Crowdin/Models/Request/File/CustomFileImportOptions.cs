using Crowdin.Api.SourceFiles;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Request.File
{
    public class CustomFileImportOptions : FileImportOptions
    {
        [JsonProperty("firstLineContainsHeader")]
        public bool? FirstLineContainsHeader { get; set; }

        [JsonProperty("importTranslations")]
        public bool? ImportTranslations { get; set; }

        [JsonProperty("importHiddenSheets")]
        public bool? ImportHiddenSheets { get; set; }

        [JsonProperty("contentSegmentation")]
        public bool? ContentSegmentation { get; set; }

        [JsonProperty("srxStorageId")]
        public long? SrxStorageId { get; set; }

        [JsonProperty("scheme")]
        public Dictionary<string, int?> Scheme { get; set; } = new();
    }
}
