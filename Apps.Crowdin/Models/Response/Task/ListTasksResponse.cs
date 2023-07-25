using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.Task;

public record ListTasksResponse(TaskEntity[] Tasks);