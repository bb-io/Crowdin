using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Response;

public class SimpleProjectResponse : CrowdinWebhookResponse<SimpleProjectWrapper>
{
    [Display("Project name")]
    public string Project { get; set; }
    
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("Target language ID")]
    public string Language { get; set; }
    
    public override void ConfigureResponse(SimpleProjectWrapper wrapper)
    {
        Project = wrapper.Project;
        ProjectId = wrapper.ProjectId;
        Language = wrapper.Language;
    }
}