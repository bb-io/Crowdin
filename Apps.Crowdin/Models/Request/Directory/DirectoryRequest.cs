using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Directory;

public class DirectoryRequest
{
    [Display("Directory ID"), DataSource(typeof(DirectoryDataHandler))]
    public string DirectoryId { get; set; } = string.Empty;
}