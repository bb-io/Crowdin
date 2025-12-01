using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.TranslationMemory
{
    public class TmSegmentRequest
    {
        [Display("Segment ID")]
        public string SegmentId { get; set; } = default!;
    }
}
