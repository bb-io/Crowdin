using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Project;

public class AddNewProjectRequest
{
    public string Name { get; set; }

    [Display("Source language")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string SourceLanguageId { get; set; }

    public string? Identifier { get; set; }

    [Display("Target language IDs")] public IEnumerable<string>? TargetLanguageIds { get; set; }
    
    [DataSource(typeof(ProjectVisibilityHandler))]
    public string? Visibility { get; set; }
    
    [Display("Custom domain name")]
    public string? Cname { get; set; }
    public string? Description { get; set; }

    [Display("Is MT allowed")] public bool? IsMtAllowed { get; set; }

    [Display("Auto substitution")] public bool? AutoSubstitution { get; set; }

    [Display("Auto translate dialects")] public bool? AutoTranslateDialects { get; set; }

    [Display("Public downloads")] public bool? PublicDownloads { get; set; }

    [Display("Hidden strings proofreaders access")]
    public bool? HiddenStringsProofreadersAccess { get; set; }

    [Display("Use global TM")] public bool? UseGlobalTm { get; set; }

    [Display("Skip untranslated strings")] public bool? SkipUntranslatedStrings { get; set; }

    [Display("Skip untranslated files")] public bool? SkipUntranslatedFiles { get; set; }

    [Display("Export approved only")] public bool? ExportApprovedOnly { get; set; }

    [Display("In context")] public bool? InContext { get; set; }

    [Display("In context process hidden strings")]
    public bool? InContextProcessHiddenStrings { get; set; }

    [Display("In context pseudo language ID")]
    public string? InContextPseudoLanguageId { get; set; }

    [Display("QA check is active")] public bool? QaCheckIsActive { get; set; }
}