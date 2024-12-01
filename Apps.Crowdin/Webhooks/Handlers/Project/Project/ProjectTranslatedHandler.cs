using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Project;

public class ProjectTranslatedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.ProjectTranslated;

    public ProjectTranslatedHandler([WebhookParameter(true)] ProjectWebhookInput input) : base(input)
    {
    }
}