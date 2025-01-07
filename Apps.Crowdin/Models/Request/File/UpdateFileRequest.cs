using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Request.File;

public class UpdateFileRequest
{
    [Display("Update option")]
    [StaticDataSource(typeof(FileUpdateOptionHandler))]
    public string? UpdateOption { get; set; }

}