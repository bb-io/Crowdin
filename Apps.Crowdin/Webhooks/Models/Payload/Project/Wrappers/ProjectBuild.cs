using Crowdin.Api.ProjectsGroups;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;

public class ProjectBuild
{
    public string Id { get; set; }
    public string DownloadLink { get; set; }
    public ProjectBase Project { get; set; }
}