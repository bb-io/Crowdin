using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Translation;

public class GetTranslationRequest
{
    [Display("Project ID")] public string ProjectId { get; set; }
    [Display("Translation ID")] public string TranslationId { get; set; }
    [Display("Denormalize placeholders")] public bool? DenormalizePlaceholders { get; set; }
}