using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Suggestions;

public class GetSuggestionOptionalRequest
{
    [Display("Suggestion ID")]
    public string? SuggestionId { get; set; }
}