using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Project.String;

public class StringUpdatedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.StringAdded;

    public StringUpdatedHandler([WebhookParameter] ProjectWebhookInput input) : base(input, true)
    {
    }
}