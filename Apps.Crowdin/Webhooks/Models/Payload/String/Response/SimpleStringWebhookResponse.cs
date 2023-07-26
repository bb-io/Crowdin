
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.String.Response;

public class SimpleStringWebhookResponse : CrowdinWebhookResponse<EventsWebhookResponse<SimpleStringWrapper>>
{
    [Display("Project name")]
    public string Project { get; set; }
    
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("String ID")]
    public string StringId { get; set; }
    
    [Display("String identifier")]
    public string StringIdentifier { get; set; }
    
    [Display("String context")]
    public string StringContext { get; set; }
    public string User { get; set; }
    
    [Display("User ID")]
    public string UserId { get; set; }
    
    public string File { get; set; }
    
    [Display("File ID")]
    public string FileId { get; set; }
    
    public override void ConfigureResponse(EventsWebhookResponse<SimpleStringWrapper> events)
    {
        var wrapper = events.Events.First();
        
        Project = wrapper.Project;
        ProjectId = wrapper.ProjectId;
        StringId = wrapper.StringId;
        StringIdentifier = wrapper.StringIdentifier;
        StringContext = wrapper.StringContext;
        User = wrapper.User;
        UserId = wrapper.UserId;
        File = wrapper.File;
        FileId = wrapper.FileId;
    }
}