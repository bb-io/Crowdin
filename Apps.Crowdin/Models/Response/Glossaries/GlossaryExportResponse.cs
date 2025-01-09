using System.Text.Json.Serialization;

namespace Apps.Crowdin.Models.Response.Glossaries;

public class GlossaryExportResponse
{
    [JsonPropertyName("data")]
    public ExportGlossaryModel Data { get; set; } = default!;
}

public class ExportGlossaryModel
{
    [JsonPropertyName("identifier")]
    public string Identifier { get; set; } = default!;
}