using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crowdin.Api.SourceFiles;

namespace Apps.Crowdin.Models.Request.File
{
    public class CustomFileImportOptions : FileImportOptions
    {
        [System.Text.Json.Serialization.JsonPropertyName("options")]
        public IDictionary<string, object> Options { get; set; } = new Dictionary<string, object>();
    }
}
