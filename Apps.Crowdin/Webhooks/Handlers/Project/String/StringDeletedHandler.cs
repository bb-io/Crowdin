using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Project.String;

public class StringDeletedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.StringDeleted;

    public StringDeletedHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}