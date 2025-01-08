using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Directory;

public class ListDirectoriesRequest
{
    [Display("Branch ID")]
    public string? BranchId { get; set; }
    
    [Display("Directory ID"), DataSource(typeof(DirectoryDataHandler))]
    public string? DirectoryId { get; set; }
    
    public string? Name { get; set; }
}