using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Dtos
{
    public class WorkflowStepsResponseDto
    {
        [JsonProperty("data")]
        public List<WorkflowStepDataWrapperDto> Data { get; set; }

        [JsonProperty("pagination")]
        public PaginationDto Pagination { get; set; }
    }
    public class WorkflowStepDataWrapperDto
    {
        [JsonProperty("data")]
        public WorkflowStepDto Data { get; set; }
    }
    public class WorkflowStepDto
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("languages")]
        public string[] Languages { get; set; }

        [JsonProperty("config")]
        public object Config { get; set; }
    }

    public class PaginationDto
    {
        [JsonProperty("offset")]
        public int Offset { get; set; }

        [JsonProperty("limit")]
        public int Limit { get; set; }
    }
}
