using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crowdin.Api.Tasks;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Request.Task
{
    public class TaskPatchOperation : TaskPatchBase
    {
        [JsonProperty("op")]
        public string Op { get; set; } = default!;

        [JsonProperty("path")]
        public string Path { get; set; } = default!;

        [JsonProperty("value")]
        public object Value { get; set; } = default!;
    }
}
