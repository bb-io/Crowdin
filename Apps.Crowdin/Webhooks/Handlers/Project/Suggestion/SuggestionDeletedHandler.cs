﻿using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Suggestion;

public class SuggestionDeletedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.SuggestionDeleted;

    public SuggestionDeletedHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}