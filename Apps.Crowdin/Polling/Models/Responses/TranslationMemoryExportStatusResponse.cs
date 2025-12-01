using Apps.Crowdin.Polling.Models.Requests;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.TranslationMemory;
using Crowdin.Api.Translations;
using Newtonsoft.Json;

namespace Apps.Crowdin.Polling.Models.Responses;

public class TranslationMemoryStatusResponse
{
    [Display("ID")]
    public string Identifier { get; set; } = default!;

    [Display("Status")]
    public string Status { get; set; }

    [Display("Created at")]
    public DateTime CreatedAt { get; set; }
    public TranslationMemoryStatusResponse() { }
    public TranslationMemoryStatusResponse(TmExportStatus response)
    {
        Identifier = response.Identifier;
        Status = response.Status.ToString();
        CreatedAt = response.CreatedAt.DateTime;
    }
    public TranslationMemoryStatusResponse(TmImportStatus response)
    {
        Identifier = response.Identifier;
        Status = response.Status.ToString();
        CreatedAt = response.CreatedAt.DateTime;
    }

}