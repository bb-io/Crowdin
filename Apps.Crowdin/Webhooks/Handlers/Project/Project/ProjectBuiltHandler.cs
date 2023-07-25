using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Project;

public class ProjectBuiltHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.ProjectBuilt;

    public ProjectBuiltHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}