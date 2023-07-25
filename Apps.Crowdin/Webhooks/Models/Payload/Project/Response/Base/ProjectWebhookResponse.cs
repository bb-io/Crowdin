using Apps.Crowdin.Models.Entities;
using Crowdin.Api.ProjectsGroups;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Response.Base;

public class ProjectWebhookResponse
{
    public ProjectEntity Project { get; set; }

    public ProjectWebhookResponse(ProjectBase project)
    {
        Project = new(project);
    }
}