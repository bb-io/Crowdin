namespace Apps.Crowdin.Webhooks.Models.Payload.File;

public class FilePayload
{
    public string Id { get; set; }
    public string? BranchId { get; set; }
    public string? DirectoryId { get; set; }
    public string Name { get; set; }
    public string? Title { get; set; }
    public string Type { get; set; }
    public string Path { get; set; }
    public string Status { get; set; }
}