namespace Apps.Crowdin.Models.Response.Glossaries;

public class ImportGlossaryResponse
{
    public string Identifier { get; set; } = default!;

    public string Status { get; set; } = default!;

    public string Progress { get; set; } = default!;
}