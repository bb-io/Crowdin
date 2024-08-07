﻿using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.String;

public class StringAddedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.StringAdded;

    public StringAddedHandler([WebhookParameter] ProjectWebhookInput input) : base(input, true)
    {
    }
}