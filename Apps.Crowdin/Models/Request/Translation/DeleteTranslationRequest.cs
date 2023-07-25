using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Translation;

public class DeleteTranslationRequest
{
    [Display("Project ID")] public string ProjectId { get; set; }
    [Display("Translation ID")] public string TranslationId { get; set; }
}