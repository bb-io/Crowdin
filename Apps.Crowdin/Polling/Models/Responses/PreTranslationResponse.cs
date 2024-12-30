using Apps.Crowdin.Polling.Models.Requests;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.Polling.Models.Responses;

public class PreTranslationResponse
{
    [Display("Pre-translation ID")]
    public string Identifier { get; set; } = default!;
    
    [Display("Pre-translation status")]
    public string Status { get; set; } = default!;
    
    public int Progress { get; set; }
    
    public Attributes Attributes { get; set; } = default!;
    
    [Display("Created at")]
    public DateTime CreatedAt { get; set; }
    
    [Display("Updated at")]
    public DateTime? UpdatedAt { get; set; }
    
    [Display("Started at")]
    public DateTime? StartedAt { get; set; }
    
    [Display("Finished at")]
    public DateTime? FinishedAt { get; set; }

    public PreTranslationResponse() { }

    public PreTranslationResponse(PreTranslation preTranslation)
    {
        Identifier = preTranslation.Identifier;
        Status = preTranslation.Status.ToReadableString();
        Progress = preTranslation.Progress;
        CreatedAt = preTranslation.CreatedAt.DateTime;
        UpdatedAt = preTranslation.UpdatedAt?.DateTime;
        StartedAt = preTranslation.StartedAt?.DateTime;
        FinishedAt = preTranslation.FinishedAt?.DateTime;
        Attributes = new Attributes
        {
            Method = preTranslation.Attributes.Method.ToString(),
            DuplicateTranslations = preTranslation.Attributes.DuplicateTranslations,
            FileIds = preTranslation.Attributes.FileIds?.ToList() ?? new(),
            LanguageIds = preTranslation.Attributes.LanguageIds.ToList(),
            LabelIds = preTranslation.Attributes.LabelIds.ToList()
        };
    }
}

public class Attributes
{
    public string Method { get; set; } = default!;
    
    [Display("File IDs")]
    public List<int> FileIds { get; set; } = default!;
    
    [Display("Label IDs")]
    public List<int> LabelIds { get; set; } = default!;
    
    [Display("Language IDs")]
    public List<string> LanguageIds { get; set; } = default!;
    
    [Display("Duplicate translations")]
    public bool DuplicateTranslations { get; set; }
}