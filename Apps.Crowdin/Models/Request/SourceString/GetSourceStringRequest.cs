using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.SourceString;

public class GetSourceStringRequest
{
    [Display("String ID")] 
    public string StringId { get; set; }

    [Display("Denormalize placeholders")] 
    public bool? DenormalizePlaceholders { get; set; }
}