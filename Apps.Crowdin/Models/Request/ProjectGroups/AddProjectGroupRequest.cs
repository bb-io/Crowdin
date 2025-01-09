using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.ProjectGroups;

public class AddProjectGroupRequest
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    [Display("Parent group ID"), DataSource(typeof(ProjectGroupDataHandler))]
    public string ParentId { get; set; } = string.Empty;
}