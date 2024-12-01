using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.File;

public class FileDeletedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.FileDeleted;

    public FileDeletedHandler([WebhookParameter(true)] ProjectWebhookInput input) : base(input)
    {
    }
}