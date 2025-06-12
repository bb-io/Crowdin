using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.File;

public class FileRequest
{
    [Display("File ID")]
    [DataSource(typeof(FileDataHandler))]
    public string FileId { get; set; }
}