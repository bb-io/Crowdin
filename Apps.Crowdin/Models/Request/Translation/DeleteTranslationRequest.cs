using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Translation;

public class DeleteTranslationRequest : ProjectRequest
{
    [Display("Translation ID")] public string TranslationId { get; set; }
}