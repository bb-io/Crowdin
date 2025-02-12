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
        public IEnumerable<int>? FileIds { get; set; }

        [Display("String IDs")]
        public IEnumerable<int>? StringIds { get; set; }

        [Display("Date from")]
        public DateTime? DateFrom { get; set; }

        [Display("Date to")]
        public DateTime? DateTo { get; set; }

        [Display("Label IDs")]
        public IEnumerable<int>? LabelIds { get; set; }

        [Display("Exclude label IDs")]
        public IEnumerable<int>? ExcludeLabelIds { get; set; }
    }
}
