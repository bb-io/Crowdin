using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.File;

public class AddOrUpdateFileRequest : AddNewFileRequest
{
    [Display("Update option")]
    [DataSource(typeof(FileUpdateOptionHandler))]
    public string? UpdateOption { get; set; }
}