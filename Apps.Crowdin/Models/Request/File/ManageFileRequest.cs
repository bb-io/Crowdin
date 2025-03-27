using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Request.File;

public class ManageFileRequest
{
    [Display("Storage ID"), DataSource(typeof(StorageDataHandler))]
    public string? StorageId { get; set; }
    
    public FileReference File { get; set; }
}