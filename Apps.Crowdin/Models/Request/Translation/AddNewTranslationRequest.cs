using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Translation;

public class AddNewTranslationRequest
{
    [Display("Project ID")] public string ProjectId { get; set; }
    [Display("Language ID")] public string LanguageId { get; set; }
    [Display("String ID")] public string StringId { get; set; }
    [Display("Text")] public string Text { get; set; }
    [Display("Plural category name")] public string? PluralCategoryName { get; set; }
}