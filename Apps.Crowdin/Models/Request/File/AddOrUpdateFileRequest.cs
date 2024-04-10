using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.Models.Request.File;

public class AddOrUpdateFileRequest : AddNewFileRequest
{
    [Display("Update option")]
    [StaticDataSource(typeof(FileUpdateOptionHandler))]
    public string? UpdateOption { get; set; }
}