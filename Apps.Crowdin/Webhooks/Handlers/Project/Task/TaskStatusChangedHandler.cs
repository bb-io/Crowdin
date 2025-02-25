using Apps.Crowdin.Factories;
using System.Net;
using Apps.Crowdin.Models.Request.Task;
using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Apps.Crowdin.Webhooks.Models.Payload.Task;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;
using Blackbird.Applications.Sdk.Common.Webhooks;
using Crowdin.Api.Tasks;
using EventType = Crowdin.Api.Webhooks.EventType;
using Blackbird.Applications.Sdk.Common.Invocation;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Api.RestSharp;
using RestSharp;
using Blackbird.Applications.Sdk.Utils.Extensions.Http;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Task;

public class TaskStatusChangedHandler(InvocationContext invocationContext,
    [WebhookParameter(true)] ProjectWebhookInput input,
    [WebhookParameter] GetTaskOptionalRequest taskOptionalRequest) : ProjectWebhookHandler(input), IAfterSubscriptionWebhookEventHandler<TaskStatusChangedWebhookResponse>
{
    protected override List<EventType> SubscriptionEvents => new() { EventType.TaskStatusChanged };

    public async Task<AfterSubscriptionEventResponse<TaskStatusChangedWebhookResponse>> OnWebhookSubscribedAsync()
    {
        await WebhookLogger.LogAsync(new
        {
            Message = "OnWebhookSubscribedAsync called (TaskStatusChanged)",
            ProjectId = input.ProjectId,
            TaskId = taskOptionalRequest.TaskId,
            DesiredStatus = taskOptionalRequest.Status
        });
        try
        {

        
        if (taskOptionalRequest.TaskId != null && input.ProjectId != null && taskOptionalRequest.Status != null)
        {

            var client = new CrowdinEnterpriseRestClient(invocationContext.AuthenticationCredentialsProviders);
            var request = new CrowdinRestRequest($"/projects/{input.ProjectId}/tasks/{taskOptionalRequest.TaskId}", Method.Get, invocationContext.AuthenticationCredentialsProviders);

            var task = await client.ExecuteWithErrorHandling<TaskStatusChangedWebhookResponse>(request);

            if (taskOptionalRequest.Status.Contains(task.Status))
            {
                return new AfterSubscriptionEventResponse<TaskStatusChangedWebhookResponse>
                {
                    Result = task
                };
            }
        }
        return null;
        }
        catch (Exception ex)
        {
            await WebhookLogger.LogAsync(new
            {
                Message = "Error in OnWebhookSubscribedAsync (TaskStatusChanged)",
                Error = ex.ToString()
            });
            throw;
        }
    }
}

