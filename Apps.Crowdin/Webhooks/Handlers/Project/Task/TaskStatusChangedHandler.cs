// Файл: TaskStatusChangedHandler.cs
using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Factories;
using Apps.Crowdin.Models.Request.Task;
using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Apps.Crowdin.Webhooks.Models.Payload.Task;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common.Webhooks;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using EventType = Crowdin.Api.Webhooks.EventType;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Task
{
    public class TaskStatusChangedHandler : ProjectWebhookHandler, IAfterSubscriptionWebhookEventHandler<TaskStatusChangedWebhookResponse>
    {
        private readonly GetTaskOptionalRequest _taskOptionalRequest;
        private readonly InvocationContext _invocationContext;
        private readonly ProjectWebhookInput _input;

        protected override List<EventType> SubscriptionEvents => new() { EventType.TaskStatusChanged };

        public TaskStatusChangedHandler(
            InvocationContext invocationContext,
            [WebhookParameter] ProjectWebhookInput input,
            [WebhookParameter] GetTaskOptionalRequest taskOptionalRequest)
            : base(input)
        {
            _invocationContext = invocationContext;
            _input = input;
            _taskOptionalRequest = taskOptionalRequest;
        }

        public async Task<AfterSubscriptionEventResponse<TaskStatusChangedWebhookResponse>> OnWebhookSubscribedAsync()
        {
            await WebhookLogger.LogAsync(new
            {
                Message = "OnWebhookSubscribedAsync called (TaskStatusChanged)",
                ProjectId = _input.ProjectId,
                TaskId = _taskOptionalRequest.TaskId,
                DesiredStatus = _taskOptionalRequest.Status
            });

            if (!string.IsNullOrEmpty(_input.ProjectId) &&
                !string.IsNullOrEmpty(_taskOptionalRequest.TaskId) &&
                !string.IsNullOrEmpty(_taskOptionalRequest.Status))
            {
                try
                {
                    var client = new CrowdinEnterpriseRestClient(_invocationContext.AuthenticationCredentialsProviders);

                    var request = new CrowdinRestRequest(
                        $"/projects/{_input.ProjectId}/tasks/{_taskOptionalRequest.TaskId}",
                        Method.Get,
                        _invocationContext.AuthenticationCredentialsProviders);

                    var taskResponse = await client.ExecuteWithErrorHandling<TaskStatusChangedWebhookResponse>(request);

                    await WebhookLogger.LogAsync(new
                    {
                        Message = "Task retrieved in OnWebhookSubscribedAsync",
                        ReturnedStatus = taskResponse.Status
                    });

                    if (_taskOptionalRequest.Status.Contains(taskResponse.Status))
                    {
                        await WebhookLogger.LogAsync(new
                        {
                            Message = "Task already in desired status, triggering Flight!",
                            CurrentStatus = taskResponse.Status
                        });

                        var wrapper = new TaskStatusChangedWrapper
                        {
                            Task = new Apps.Crowdin.Webhooks.Models.Payload.Task.TaskStatusChangedPayload
                            {
                                Id = taskResponse.Id,
                                Status = taskResponse.Status,
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
                            CurrentStatus = taskResponse.Status
                        });
                    }
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
            return null;
        }
    }
}
