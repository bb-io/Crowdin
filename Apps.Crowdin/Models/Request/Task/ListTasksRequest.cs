using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.Models.Request.Task;

public class ListTasksRequest : ProjectRequest
{
    [StaticDataSource(typeof(TaskStatusHandler))]
    public string? Status { get; set; }
    
    [Display("Assignee ID")]
    public string? AssigneeId { get; set; }
}