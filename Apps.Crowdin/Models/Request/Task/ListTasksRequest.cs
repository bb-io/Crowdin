using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Task;

public class ListTasksRequest : ProjectRequest
{
    [StaticDataSource(typeof(TaskStatusHandler))]
    public string? Status { get; set; }

    [DataSource(typeof(ProjectMemberDataSourceHandler))]
    [Display("Assignee ID")]
    public string? AssigneeId { get; set; }
}