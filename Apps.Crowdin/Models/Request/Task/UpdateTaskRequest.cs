using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.Task
{
    public class UpdateTaskRequest
    {
        [Display("Status")]
        public string? Status { get; set; }

        [Display("Title")]
        public string? Title { get; set; }

        [Display("Description")]
        public string? Description { get; set; }

        [Display("Deadline")]
        public DateTime? Deadline { get; set; }

        [Display("Started at")]
        public DateTime? StartedAt { get; set; }

        [Display("Resolved at")]
        public DateTime? ResolvedAt { get; set; }

        [Display("File IDs")]
        public IEnumerable<string>? FileIds { get; set; }

        [Display("String IDs")]
        public IEnumerable<string>? StringIds { get; set; }

        [Display("Date from")]
        public DateTime? DateFrom { get; set; }

        [Display("Date to")]
        public DateTime? DateTo { get; set; }

        [Display("Label IDs")]
        public IEnumerable<string>? LabelIds { get; set; }

        [Display("Exclude label IDs")]
        public IEnumerable<string>? ExcludeLabelIds { get; set; }
    }
}
