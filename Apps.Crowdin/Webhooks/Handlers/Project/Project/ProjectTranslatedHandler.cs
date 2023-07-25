using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Project;

public class ProjectTranslatedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.ProjectTranslated;

    public ProjectTranslatedHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}