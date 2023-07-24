using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.TranslationMemory;

namespace Apps.Crowdin.Models.Entities;

public class TmExportEntity
{
    [Display("ID")]
    public string Id { get; set; }

    public string Status { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }

    public TmExportEntity(TmExportStatus response)
    {
        Id = response.Identifier;
        Status = response.Status.ToString();
        CreatedAt = response.CreatedAt.DateTime;
    }
}