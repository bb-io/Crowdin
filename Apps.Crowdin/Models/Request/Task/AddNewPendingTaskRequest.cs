using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Task;

public class AddNewPendingTaskRequest
{
    public string Title { get; set; }

    [Display("Preceding task ID")]
    public string PrecedingTask { get; set; }

    [StaticDataSource(typeof(PendingTaskTypeHandler))]
    public string Type { get; set; }

    public string? Description { get; set; }
    
    [DataSource(typeof(ProjectMemberDataSourceHandler))]
    public IEnumerable<string>? Assignees { get; set; }

    public DateTime? Deadline { get; set; }
}