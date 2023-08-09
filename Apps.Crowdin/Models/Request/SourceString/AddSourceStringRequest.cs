using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.SourceString;

public class AddSourceStringRequest : ProjectRequest
{
    [Display("File ID")] public string FileId { get; set; }
    public string Text { get; set; }
    public string Identifier { get; set; }
    public string? Context { get; set; }
    [Display("Is hidden")] public bool? IsHidden { get; set; }
    [Display("Max length")] public int? MaxLength { get; set; }
    [Display("Label IDs")] public IEnumerable<string>? LabelIds { get; set; }
}