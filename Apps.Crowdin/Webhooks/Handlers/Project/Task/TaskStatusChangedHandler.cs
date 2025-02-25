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
using Apps.Crowdin.Webhooks.Lists;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Task;

public class TaskStatusChangedHandler([WebhookParameter(true)] ProjectWebhookInput input,
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

        if (!string.IsNullOrEmpty(input.ProjectId) &&
            !string.IsNullOrEmpty(taskOptionalRequest.TaskId) &&
            !string.IsNullOrEmpty(taskOptionalRequest.Status))
        {
            try
            {
                var context = CrowdinContextHolder.Current;
                if (context == null)
                    throw new System.Exception("InvocationContext is not available in CrowdinContextHolder.");

                var client = new Apps.Crowdin.Api.RestSharp.Enterprise.CrowdinEnterpriseRestClient(context.AuthenticationCredentialsProviders);

                var request = new Apps.Crowdin.Api.RestSharp.CrowdinRestRequest(
                    $"/projects/{input.ProjectId}/tasks/{taskOptionalRequest.TaskId}",
                    Method.Get,
                    context.AuthenticationCredentialsProviders);

                var task = await client.ExecuteWithErrorHandling<TaskStatusChangedWebhookResponse>(request);

                await WebhookLogger.LogAsync(new
                {
                    Message = "Task retrieved in OnWebhookSubscribedAsync",
                    ReturnedStatus = task.Status
                });

                if (taskOptionalRequest.Status.Contains(task.Status))
                {
                    await WebhookLogger.LogAsync(new
                    {
                        Message = "Task already in desired status, triggering Flight!",
                        CurrentStatus = task.Status
                    });

                    var wrapper = new TaskStatusChangedWrapper
                    {
                        Task = new Apps.Crowdin.Webhooks.Models.Payload.Task.TaskStatusChangedPayload
                        {
                            Id = task.Id,
                            Status = task.Status,
                        }
                    };

                    var responseDto = new TaskStatusChangedWebhookResponse();
                    responseDto.ConfigureResponse(wrapper);

                    return new AfterSubscriptionEventResponse<TaskStatusChangedWebhookResponse>
                    {
                        Result = responseDto
                    };
                }
                else
                {
                    await WebhookLogger.LogAsync(new
                    {
                        Message = "Task status does not match the desired status",
                        CurrentStatus = task.Status
                    });
                }
            }
            catch (System.Exception ex)
            {
                await WebhookLogger.LogAsync(new
                {
                    Message = "Error in OnWebhookSubscribedAsync (TaskStatusChanged)",
                    Error = ex.ToString()
                });
                throw;
            }
        }
        return null;
    }
}