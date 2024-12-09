using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Suggestion;

public class SuggestionApprovedHandler([WebhookParameter(true)] ProjectWebhookInput input)
    : ProjectWebhookHandler(input)
{
    protected override List<EventType> SubscriptionEvents => new() { EventType.SuggestionApproved };
}