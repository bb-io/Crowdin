using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload.Task.Wrapper;
using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Tasks;

namespace Apps.Crowdin.Webhooks.Models.Payload.Task.Response;

public class TaskStatusChangedWebhookResponse : CrowdinWebhookResponse<TaskStatusChangedWrapper>
{
    [Display("ID")] public string Id { get; set; }
    [Display("Type")] public string Type { get; set; }
    [Display("Vendor")] public string Vendor { get; set; }
    [Display("Old status")] public string OldStatus { get; set; }
    [Display("New status")] public string NewStatus { get; set; }
    [Display("Status")] public string Status { get; set; }
    [Display("Title")] public string Title { get; set; }
    [Display("Assignee IDs")] public IEnumerable<string>? AssigneeIds { get; set; }
    [Display("File IDs")] public IEnumerable<string> FileIds { get; set; }
    [Display("Progress")] public TaskProgress Progress { get; set; }
    [Display("Description")] public string Description { get; set; }
    [Display("Translation URL")] public string TranslationUrl { get; set; }
    [Display("Deadline")] public DateTime? Deadline { get; set; }
    [Display("Created at")] public DateTime CreatedAt { get; set; }
    [Display("Source language ID")] public string SourceLanguageId { get; set; }
    [Display("Target language ID")] public string TargetLanguageId { get; set; }
    [Display("Project ID")] public string ProjectId { get; set; }
    [Display("Task creator")] public UserEntity TaskCreator { get; set; }

    public override void ConfigureResponse(TaskStatusChangedWrapper wrapper)
    {
        Id = wrapper.Task.Id;
        Type = wrapper.Task.Type;
        Vendor = wrapper.Task.Vendor;
        OldStatus = wrapper.Task.OldStatus;
        NewStatus = wrapper.Task.NewStatus;
        Status = wrapper.Task.Status;
        Title = wrapper.Task.Title;
        AssigneeIds = wrapper.Task.Assignees?.Select(x => x.Id.ToString()) ?? new List<string>();
        FileIds = wrapper.Task.FileIds?.Select(x => x.ToString()) ?? new List<string>();
        Progress = wrapper.Task.Progress;
        Description = wrapper.Task.Description;
        TranslationUrl = wrapper.Task.TranslationUrl;
        Deadline = wrapper.Task.Deadline;
        CreatedAt = wrapper.Task.CreatedAt;
        SourceLanguageId = wrapper.Task.SourceLanguage.Id;
        TargetLanguageId = wrapper.Task.TargetLanguage.Id;
        ProjectId = wrapper.Task.Project.Id.ToString();
        TaskCreator = new(wrapper.Task.TaskCreator);
    }
}