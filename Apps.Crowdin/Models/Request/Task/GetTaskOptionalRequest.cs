using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Task;

public class GetTaskOptionalRequest
{
    [Display("Task ID")]
    public string? TaskId { get; set; }
}