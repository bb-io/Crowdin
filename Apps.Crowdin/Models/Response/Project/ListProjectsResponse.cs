using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.Project;

public record ListProjectsResponse(List<ProjectEntity> Projects);