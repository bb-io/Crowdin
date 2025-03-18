using Apps.Crowdin.Models.Request.Task;
using Apps.Crowdin.Webhooks.Handlers.Base;
using Apps.Crowdin.Webhooks.Models.Inputs;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Response;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;
using Blackbird.Applications.Sdk.Common.Webhooks;
using EventType = Crowdin.Api.Webhooks.EventType;
using Blackbird.Applications.Sdk.Common.Invocation;
using RestSharp;
using Apps.Crowdin.Models.Dtos;
using Apps.Crowdin.Webhooks.Models.Payload.Task;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Task;

public class TaskStatusChangedHandler(InvocationContext invocationContext, [WebhookParameter(true)] ProjectWebhookInput input,
    [WebhookParameter] GetTaskOptionalRequest taskOptionalRequest) : ProjectWebhookHandler(invocationContext, input), IAfterSubscriptionWebhookEventHandler<TaskStatusChangedWebhookResponse>
{
    protected override List<EventType> SubscriptionEvents => new() { EventType.TaskStatusChanged };

    public async Task<AfterSubscriptionEventResponse<TaskStatusChangedWebhookResponse>> OnWebhookSubscribedAsync()
    {
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

                var task = await client.ExecuteWithErrorHandling<DataWrapperDto<TaskStatusChangedPayload>>(request);

                if (taskOptionalRequest.Status.Contains(task.Data.Status))
                {
                    var wrapper = new TaskStatusChangedWrapper
                    {
                        Task = new Apps.Crowdin.Webhooks.Models.Payload.Task.TaskStatusChangedPayload
                        {
                            Id = task.Data.Id,
                            Status = task.Data.Status,
                            Type = task.Data.Type,                        
                            Vendor = task.Data.Vendor,                     
                            OldStatus = task.Data.OldStatus,              
                            NewStatus = task.Data.NewStatus,              
                            Title = task.Data.Title,                      
                            Assignees = task.Data.Assignees,               
                            FileIds = task.Data.FileIds,                 
                            Progress = task.Data.Progress,               
                            Description = task.Data.Description,          
                            TranslationUrl = task.Data.TranslationUrl,    
                            Deadline = task.Data.Deadline,                 
                            CreatedAt = task.Data.CreatedAt,              
                            SourceLanguage = task.Data.SourceLanguage,    
                            TargetLanguage = task.Data.TargetLanguage,     
                            Project = task.Data.Project,                   
                            TaskCreator = task.Data.TaskCreator            
                        }
                    };

                    var responseDto = new TaskStatusChangedWebhookResponse();
                    responseDto.ConfigureResponse(wrapper);

                    return new AfterSubscriptionEventResponse<TaskStatusChangedWebhookResponse>
                    {
                        Result = responseDto
                    };
                }
            }
            catch (System.Exception ex)
            {
                throw;
            }
        }
        return null;
    }
}