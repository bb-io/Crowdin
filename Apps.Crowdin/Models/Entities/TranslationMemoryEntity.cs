using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.TranslationMemory;

namespace Apps.Crowdin.Models.Entities;

public class TranslationMemoryEntity
{
    [Display("ID")] public string Id { get; set; }

    [Display("User ID")] public string UserId { get; set; }
    
    public string Name { get; set; }

    [Display("Language IDs")] public IEnumerable<string> LanguageIds { get; set; }

    [Display("Segments count")] public int SegmentsCount { get; set; }

    [Display("Created at")] public DateTime CreatedAt { get; set; }

    public TranslationMemoryEntity(TranslationMemory tm)
    {
        Id = tm.Id.ToString();
        UserId = tm.UserId.ToString();
        Name = tm.Name;
        LanguageIds = tm.LanguageIds;
        SegmentsCount = tm.SegmentsCount;
        CreatedAt = tm.CreatedAt.DateTime;
    }
}