using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.TranslationMemory;

namespace Apps.Crowdin.Models.Entities;

public class TmSegmentRecordEntity
{
    [Display("ID")]
    public string Id { get; set; }

    [Display("Language ID")]
    public string LanguageId { get; set; }

    public string Text { get; set; }

    [Display("Usage count")]
    public int UsageCount { get; set; }

    [Display("Created by")]
    public string CreatedBy { get; set; }

    [Display("Updated by")]
    public string? UpdatedBy { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    public TmSegmentRecordEntity()
    {
    }

    public TmSegmentRecordEntity(TmSegmentRecord record)
    {
        Id = record.Id.ToString();
        LanguageId = record.LanguageId;
        Text = record.Text;
        UsageCount = record.UsageCount;
        CreatedBy = record.CreatedBy.ToString();
        UpdatedBy = record.UpdatedBy?.ToString();
        CreatedAt = record.CreatedAt.DateTime;
    }
}