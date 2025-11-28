using Apps.Crowdin.Models.Entities;
using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.TranslationMemory;

namespace Apps.Crowdin.Models.Request.TranslationMemory
{
    public class TmSegmentEntity
    {
        [Display("Segment ID")]
        public long Id { get; set; }

        [Display("Records")]
        public IEnumerable<TmSegmentRecordEntity> Records { get; set; }
            = Enumerable.Empty<TmSegmentRecordEntity>();

        public TmSegmentEntity()
        {
        }

        public TmSegmentEntity(TmSegmentResource source, IEnumerable<TmSegmentRecordEntity>? recordsOverride = null)
        {
            Id = source.Id;
            Records = recordsOverride
                      ?? (source.Records?.Select(r => new TmSegmentRecordEntity(r))
                          ?? Enumerable.Empty<TmSegmentRecordEntity>());
        }
    }
}
