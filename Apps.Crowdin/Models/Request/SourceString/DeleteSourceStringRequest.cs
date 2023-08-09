using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.SourceString;

public class DeleteSourceStringRequest : ProjectRequest
{
    [Display("String ID")] public string StringId { get; set; }
}