using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Webhooks;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Task;

public class TaskDeletedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.TaskDeleted;

    public TaskDeletedHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}