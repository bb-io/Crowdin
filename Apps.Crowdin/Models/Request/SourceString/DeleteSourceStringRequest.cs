using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.SourceString;

public class DeleteSourceStringRequest
{
    [Display("Project ID")] public string ProjectId { get; set; }
    [Display("String ID")] public string StringId { get; set; }
}