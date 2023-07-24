using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.Models.Entities;

public class PreTranslationEntity
{
    [Display("ID")] public string Id { get; set; }

    public string Status { get; set; }

    public PreTranslationEntity(PreTranslation preTranslation)
    {
        Id = preTranslation.Identifier;
        Status = preTranslation.Status.ToString();
    }
}