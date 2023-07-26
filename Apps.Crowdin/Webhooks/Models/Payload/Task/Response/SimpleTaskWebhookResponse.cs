using Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Task.Response;

public class SimpleTaskWebhookResponse : CrowdinWebhookResponse<SimpleTaskWrapper>
{
    [Display("Project name")]
    public string Project { get; set; }
    
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("Target language ID")]
    public string Language { get; set; }
    
    [Display("Task ID")]
    public string TaskId { get; set; }
    
    public string User { get; set; }
    
    [Display("User ID")]
    public string UserId { get; set; }
    
    public override void ConfigureResponse(SimpleTaskWrapper wrapper)
    {
        Project = wrapper.Project;
        ProjectId = wrapper.ProjectId;
        Language = wrapper.Language;
        TaskId = wrapper.TaskId;
        User = wrapper.User;
        UserId = wrapper.UserId;
    }
}