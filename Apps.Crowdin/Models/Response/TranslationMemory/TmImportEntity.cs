using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.TranslationMemory;

namespace Apps.Crowdin.Models.Response.TranslationMemory
{
    public class TmImportEntity
    {
        [Display("Import ID")]
        public string ImportId { get; set; } = default!;

        [Display("Status")]
        public string Status { get; set; } = default!;

        [Display("Progress (%)")]
        public int Progress { get; set; }

        [Display("Created at")]
        public DateTimeOffset CreatedAt { get; set; }

        [Display("Started at")]
        public DateTimeOffset? StartedAt { get; set; }

        [Display("Finished at")]
        public DateTimeOffset? FinishedAt { get; set; }

        [Display("Updated at")]
        public DateTimeOffset? UpdatedAt { get; set; }

        public TmImportEntity()
        {
        }

        public TmImportEntity(TmImportStatus status)
        {
            ImportId = status.Identifier;
            Status = status.Status.ToString();
            Progress = status.Progress;
            CreatedAt = status.CreatedAt;
            StartedAt = status.StartedAt;
            FinishedAt = status.FinishedAt;
            UpdatedAt = status.UpdatedAt;
        }
    }
}
