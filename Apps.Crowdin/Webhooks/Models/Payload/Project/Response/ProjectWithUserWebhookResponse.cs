using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Response.Base;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;

namespace Apps.Crowdin.Webhooks.Models.Payload.Project.Response;

public class ProjectWithUserWebhookResponse : ProjectWebhookResponse
{
    public UserEntity User { get; set; }

    public ProjectWithUserWebhookResponse(ProjectWithUserWrapper projectWrapper) : base(projectWrapper.Project)
    {
        User = new(projectWrapper.User);
    }
}