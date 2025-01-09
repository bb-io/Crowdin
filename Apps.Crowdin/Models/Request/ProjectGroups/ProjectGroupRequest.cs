using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.ProjectGroups;

public class ProjectGroupRequest
{
    [Display("Group ID"), DataSource(typeof(ProjectGroupDataHandler))]
    public string? ProjectGroupId { get; set; } 
}