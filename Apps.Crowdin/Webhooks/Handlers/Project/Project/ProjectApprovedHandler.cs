using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Project;

public class ProjectApprovedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.ProjectApproved;

    public ProjectApprovedHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}