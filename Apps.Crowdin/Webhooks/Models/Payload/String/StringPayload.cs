using Apps.Crowdin.Webhooks.Models.Payload.File;
using Crowdin.Api.ProjectsGroups;

namespace Apps.Crowdin.Webhooks.Models.Payload.String;

public class StringPayload
{
    public string Id { get; set; }
    public string Identifier { get; set; }
    public string Text { get; set; }
    public string Type { get; set; }
    public string Context { get; set; }
    public string Url { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public FilePayload File { get; set; }
    public EnterpriseProject Project { get; set; }
}