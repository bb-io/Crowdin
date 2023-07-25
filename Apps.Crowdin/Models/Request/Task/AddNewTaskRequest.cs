using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Task;

public class AddNewTaskRequest
{
    public string Title { get; set; }

    [Display("Language ID")]
    public string LanguageId { get; set; }

    [Display("File IDs")]
    public IEnumerable<string> FileIds { get; set; }

    public string Type { get; set; }

    public string? Status { get; set; }

    public string? Description { get; set; }

    [Display("Split files")]
    public bool? SplitFiles { get; set; }

    [Display("Skip assigned strings")]
    public bool? SkipAssignedStrings { get; set; }

    [Display("Skip untranslated strings")]
    public bool? SkipUntranslatedStrings { get; set; }

    [Display("Include pre-translated strings only")]
    public bool? IncludePreTranslatedStringsOnly { get; set; }

    [Display("Label IDs")]
    public IEnumerable<string>? LabelIds { get; set; }

    [Display("Assignee IDs")]
    public IEnumerable<string>? Assignees { get; set; }

    public DateTime? Deadline { get; set; }

    [Display("Date from")]
    public DateTime? DateFrom { get; set; }

    [Display("Date to")]
    public DateTime? DateTo { get; set; }
}