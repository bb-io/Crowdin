using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.Models.Entities;

public class PreTranslationEntity(PreTranslation preTranslation)
{
    [Display("Pre-translation ID")] 
    public string Id { get; set; } = preTranslation.Identifier;

    [Display("Pre-translation status")] 
    public string Status { get; set; } = preTranslation.Status.ToString();
}