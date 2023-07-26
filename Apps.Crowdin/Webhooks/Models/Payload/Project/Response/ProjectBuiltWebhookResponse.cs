using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Response;

public class ProjectBuiltWebhookResponse : CrowdinWebhookResponse<ProjectBuildWrapper>
{
    public ProjectEntity Project { get; set; }
    [Display("Build ID")] public string BuildId { get; set; }
    [Display("Build download URL")] public string BuildDownloadUrl { get; set; }

    public override void ConfigureResponse(ProjectBuildWrapper wrapper)
    {
        Project = new(wrapper.Build.Project);
        BuildId = wrapper.Build.Id;
        BuildDownloadUrl = wrapper.Build.DownloadLink;
    }
}