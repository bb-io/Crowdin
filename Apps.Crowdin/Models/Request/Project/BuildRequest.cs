using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Project;

public class BuildRequest
{
    [Display("Build ID")]
    public string BuildId { get; set; } = default!;
}