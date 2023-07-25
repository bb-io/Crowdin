using Apps.Crowdin.Webhooks.Handlers.Base;
using Crowdin.Api.Webhooks.Organization;

namespace Apps.Crowdin.Webhooks.Handlers.Organization;

public class ProjectCreatedHandler : OrganizationWebhookHandler
{
    protected override OrganizationEventType SubscriptionEvent => OrganizationEventType.ProjectCreated;
}