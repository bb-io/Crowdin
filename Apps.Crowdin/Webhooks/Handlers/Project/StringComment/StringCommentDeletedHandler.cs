using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.StringComment;

public class StringCommentDeletedHandler : ProjectWebhookHandler
{
    protected override EventType SubscriptionEvent => EventType.StringCommentDeleted;

    public StringCommentDeletedHandler([WebhookParameter] ProjectWebhookInput input) : base(input)
    {
    }
}