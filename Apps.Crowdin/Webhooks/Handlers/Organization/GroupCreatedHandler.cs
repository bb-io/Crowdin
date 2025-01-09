using Apps.Crowdin.Webhooks.Handlers.Base;
using Crowdin.Api.Webhooks.Organization;

namespace Apps.Crowdin.Webhooks.Handlers.Organization;

public class GroupCreatedHandler : EnterpriseOrganizationWebhookHandler
{
    protected override EnterpriseOrgEventType SubscriptionEvent => EnterpriseOrgEventType.GroupCreated;
}