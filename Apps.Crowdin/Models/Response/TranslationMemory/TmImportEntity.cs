using Crowdin.Api.TranslationMemory;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.TranslationMemory;

public class TmImportEntity(TmImportStatus status)
{
    [Display("Import ID")]
    public string ImportId { get; set; } = status.Identifier;

    [Display("Status")]
    public string Status { get; set; } = status.Status.ToString();

    [Display("Progress (%)")]
    public int Progress { get; set; } = status.Progress;

    [Display("Created at")]
    public DateTime CreatedAt { get; set; } = status.CreatedAt.UtcDateTime;

    [Display("Started at")]
    public DateTime? StartedAt { get; set; } = status.StartedAt?.UtcDateTime;

    [Display("Finished at")]
    public DateTime? FinishedAt { get; set; } = status.FinishedAt?.UtcDateTime;

    [Display("Updated at")]
    public DateTime? UpdatedAt { get; set; } = status.UpdatedAt?.UtcDateTime;
}
