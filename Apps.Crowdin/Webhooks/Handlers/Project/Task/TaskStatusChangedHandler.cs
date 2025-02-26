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
using Apps.Crowdin.Invocables;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Task;

public class TaskStatusChangedHandler(InvocationContext invocationContext, [WebhookParameter(true)] ProjectWebhookInput input,
    [WebhookParameter] GetTaskOptionalRequest taskOptionalRequest) : ProjectWebhookHandler(invocationContext, input), IAfterSubscriptionWebhookEventHandler<TaskStatusChangedWebhookResponse>
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
                var context = invocationContext.AuthenticationCredentialsProviders;
                if (context == null)
                    throw new System.Exception("InvocationContext is not available in CrowdinContextHolder.");

                var client = new Apps.Crowdin.Api.RestSharp.Enterprise.CrowdinEnterpriseRestClient(context);

                var request = new Apps.Crowdin.Api.RestSharp.CrowdinRestRequest(
                    $"/projects/{input.ProjectId}/tasks/{taskOptionalRequest.TaskId}",
                    Method.Get,
                    context);

                var task = await client.ExecuteWithErrorHandling<TaskStatusChangedWrapper>(request);

                await WebhookLogger.LogAsync(new
                {
                    Message = "Task retrieved in OnWebhookSubscribedAsync",
                    ReturnedStatus = task.Task.Status
                });

                if (taskOptionalRequest.Status.Contains(task.Task.Status))
                {
                    await WebhookLogger.LogAsync(new
                    {
                        Message = "Task already in desired status, triggering Flight!",
                        CurrentStatus = task.Task.Status
                    });

                    var wrapper = new TaskStatusChangedWrapper
                    {
                        Task = new Apps.Crowdin.Webhooks.Models.Payload.Task.TaskStatusChangedPayload
                        {
                            Id = task.Task.Id,
                            Status = task.Task.Status,
                            Type = task.Task.Type,                        
                            Vendor = task.Task.Vendor,                     
                            OldStatus = task.Task.OldStatus,              
                            NewStatus = task.Task.NewStatus,              
                            Title = task.Task.Title,                      
                            Assignees = task.Task.Assignees,               
                            FileIds = task.Task.FileIds,                 
                            Progress = task.Task.Progress,               
                            Description = task.Task.Description,          
                            TranslationUrl = task.Task.TranslationUrl,    
                            Deadline = task.Task.Deadline,                 
                            CreatedAt = task.Task.CreatedAt,              
                            SourceLanguage = task.Task.SourceLanguage,    
                            TargetLanguage = task.Task.TargetLanguage,     
                            Project = task.Task.Project,                   
                            TaskCreator = task.Task.TaskCreator            
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
                        CurrentStatus = task.Task.Status
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