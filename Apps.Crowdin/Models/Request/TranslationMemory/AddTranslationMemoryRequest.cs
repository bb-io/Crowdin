using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.TranslationMemory;

public class AddTranslationMemoryRequest
{
    public string Name { get; set; }
    [Display("Language ID")] public string LanguageId { get; set; }
    [Display("Group ID")] public string? GroupId { get; set; }
}