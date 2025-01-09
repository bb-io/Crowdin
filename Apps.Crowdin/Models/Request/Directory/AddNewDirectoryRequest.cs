using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Directory;

public class AddNewDirectoryRequest
{
    public string Path { get; set; }
    
    public string? Title { get; set; }
    
    [Display("Path contains file")]
    public bool? PathContainsFile { get; set; }
    
    [Display("Branch ID")] 
    public string? BranchId { get; set; }
    
    [Display("Directory ID"), DataSource(typeof(DirectoryDataHandler))] 
    public string? DirectoryId { get; set; }
}