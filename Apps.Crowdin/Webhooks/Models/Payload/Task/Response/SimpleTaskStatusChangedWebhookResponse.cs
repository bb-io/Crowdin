using Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Task.Response;

public class SimpleTaskStatusChangedWebhookResponse : CrowdinWebhookResponse<SimpleTaskStatusChangedWrapper>
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
    
    [Display("Old status")]
    public string OldStatus { get; set; }
    
    [Display("New status")]
    public string NewStatus { get; set; }
    
    public override void ConfigureResponse(SimpleTaskStatusChangedWrapper wrapper)
    {
        Project = wrapper.Project;
        ProjectId = wrapper.ProjectId;
        Language = wrapper.Language;
        TaskId = wrapper.TaskId;
        User = wrapper.User;
        UserId = wrapper.UserId;
        OldStatus = wrapper.OldStatus;
        NewStatus = wrapper.NewStatus;
    }
}