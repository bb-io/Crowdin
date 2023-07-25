using Apps.Crowdin.Webhooks.Handlers.Base;
using Crowdin.Api.Webhooks.Organization;

namespace Apps.Crowdin.Webhooks.Handlers.Organization;

public class ProjectDeletedHandler : OrganizationWebhookHandler
{
    protected override OrganizationEventType SubscriptionEvent => OrganizationEventType.ProjectDeleted;
}