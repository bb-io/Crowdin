using Apps.Crowdin.Models.Entities;

using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Response.Task;

public record ListTasksResponse(List<TaskEntity> Tasks)
{
    public List<TaskEntity> Tasks { get; set; } = Tasks;

    [Display("Task IDs")]
    public IEnumerable<string> TaskIds { get; set; } = Tasks.Select(x => x.Id).ToList();
}
