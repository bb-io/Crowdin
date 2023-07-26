using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.String;

public class StringWebhookResponseEntity
{
    [Display("ID")] public string Id { get; set; }
    [Display("Project ID")] public string ProjectId { get; set; }
    [Display("File ID")] public string FileId { get; set; }
    public string Identifier { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }
    public string Context { get; set; }
    public string Url { get; set; }

    [Display("Created at")] public DateTime CreatedAt { get; set; }

    public StringWebhookResponseEntity(StringPayload payload)
    {
        Id = payload.Id;
        ProjectId = payload.Project.Id.ToString();
        FileId = payload.File.Id.ToString();
        Identifier = payload.Identifier;
        Text = payload.Text;
        Type = payload.Type;
        Context = payload.Context;
        Url = payload.Url;
        CreatedAt = payload.CreatedAt;
    }
}