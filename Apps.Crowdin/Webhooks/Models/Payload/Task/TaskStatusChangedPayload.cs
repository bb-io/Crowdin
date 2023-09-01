namespace Apps.Crowdin.Webhooks.Models.Payload.Task;

public class TaskStatusChangedPayload : TaskPayload
{
    public string OldStatus { get; set; }
    public string NewStatus { get; set; }
}