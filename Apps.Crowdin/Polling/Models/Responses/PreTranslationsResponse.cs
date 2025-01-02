using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Polling.Models.Responses;

public class PreTranslationsResponse(List<PreTranslationResponse> preTranslations)
{
    [Display("Pre-translations")]
    public List<PreTranslationResponse> PreTranslations { get; set; } = preTranslations;
}