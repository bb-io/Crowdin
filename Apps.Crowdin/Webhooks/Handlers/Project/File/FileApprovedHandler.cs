﻿using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.File;

public class FileApprovedHandler(InvocationContext invocationContext, [WebhookParameter(true)] ProjectWebhookInput input) : ProjectWebhookHandler(invocationContext, input)
{
    protected override List<EventType> SubscriptionEvents => new() { EventType.FileApproved };
}