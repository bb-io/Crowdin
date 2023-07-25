using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Task;

public class ListTasksRequest
{
    [Display("Project ID")]
    public string ProjectId { get; set; }
    public string? Status { get; set; }
    
    [Display("Assignee ID")]
    public string? AssigneeId { get; set; }
}