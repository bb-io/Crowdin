using Crowdin.Api.Tasks;
using Newtonsoft.Json;

namespace Apps.Crowdin.Models.Request.Task;

public class CrowdinTaskCreateForm : TaskCreateForm
{
    [JsonProperty("vendor")]
    public string? Vendor { get; set; }
}