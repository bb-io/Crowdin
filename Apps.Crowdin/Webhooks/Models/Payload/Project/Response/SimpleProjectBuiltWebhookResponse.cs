using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Response;

public class SimpleProjectBuiltWebhookResponse : CrowdinWebhookResponse<SimpleProjectBuildWrapper>
{
    [Display("Project name")]
    public string Project { get; set; }
    
    [Display("Project ID")]
    public string ProjectId { get; set; }
    
    [Display("Project build ID")]
    public string ProjectBuildId { get; set; }
    
    [Display("Project build download URL")]
    public string ProjectBuildIdDownloadUrl { get; set; }
    
    public override void ConfigureResponse(SimpleProjectBuildWrapper wrapper)
    {
        Project = wrapper.Project;
        ProjectId = wrapper.ProjectId;
        ProjectBuildId = wrapper.ProjectBuildId;
        ProjectBuildIdDownloadUrl = wrapper.ProjectBuildIdDownloadUrl;
    }
}