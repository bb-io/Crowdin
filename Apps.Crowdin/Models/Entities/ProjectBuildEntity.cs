using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.Models.Entities;

public class ProjectBuildEntity(ProjectBuild build)
{
    [Display("Build ID")]
    public string Id { get; set; } = build.Id.ToString();

    [Display("Project ID")]
    public string ProjectId { get; set; } = build.ProjectId.ToString();

    public string Status { get; set; } = build.Status.ToString();

    [Display("Created at")]
    public DateTime CreatedAt { get; set; } = build.CreatedAt.DateTime;
}