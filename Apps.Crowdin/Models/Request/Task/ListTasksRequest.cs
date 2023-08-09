using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Task;

public class ListTasksRequest : ProjectRequest
{
    [DataSource(typeof(TaskStatusHandler))]
    public string? Status { get; set; }
    
    [Display("Assignee ID")]
    public string? AssigneeId { get; set; }
}