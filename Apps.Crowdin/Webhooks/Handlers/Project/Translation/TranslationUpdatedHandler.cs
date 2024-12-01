using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Translation;

public class TranslationUpdatedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.TranslationUpdated;

    public TranslationUpdatedHandler([WebhookParameter(true)] ProjectWebhookInput input) : base(input)
    {
    }
}