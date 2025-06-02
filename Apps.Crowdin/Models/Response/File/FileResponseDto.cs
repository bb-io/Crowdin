using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Response.File
{
    public class FileResponseDto
    {
        [JsonProperty("data")]
        public FileDto Data { get; set; }
    }
    public class FileDto
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("projectId")]
        public long ProjectId { get; set; }

        [JsonProperty("branchId")]
        public long? BranchId { get; set; }

        [JsonProperty("directoryId")]
        public long? DirectoryId { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("context")]
        public string Context { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("path")]
        public string Path { get; set; }

        [JsonProperty("status")]
        public string Status { get; set; }
    }
    public class FileDetailsEntity
    {
        [Display("File ID")]
        [JsonProperty("id")]
        public long Id { get; set; }

        [Display("Project ID")]
        [JsonProperty("projectId")]
        public long ProjectId { get; set; }

        [Display("Branch ID")]
        [JsonProperty("branchId")]
        public long? BranchId { get; set; }

        [Display("Directory ID")]
        [JsonProperty("directoryId")]
        public long? DirectoryId { get; set; }

        [Display("Name")]
        [JsonProperty("name")]
        public string Name { get; set; }

        [Display("Title")]
        [JsonProperty("title")]
        public string Title { get; set; }

        [Display("Context")]
        [JsonProperty("context")]
        public string Context { get; set; }

        [Display("Type")]
        [JsonProperty("type")]
        public string Type { get; set; }

        [Display("Path")]
        [JsonProperty("path")]
        public string Path { get; set; }

        [Display("Status")]
        [JsonProperty("status")]
        public string Status { get; set; }
    }
}
