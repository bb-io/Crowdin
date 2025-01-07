using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.ProjectsGroups;

namespace Apps.Crowdin.Models.Entities;

public class GroupEntity(Group group)
{
    [Display("ID")]
    public string Id { get; set; } = group.Id.ToString();

    [Display("Name")]
    public string Name { get; set; } = group.Name;

    [Display("Created at")]
    public DateTime CreatedAt { get; set; } = group.CreatedAt.DateTime;

    [Display("Updated at")]
    public DateTime? UpdatedAt { get; set; } = group.UpdatedAt?.DateTime;

    [Display("Description")]
    public string Description { get; set; } = group.Description;

    [Display("Parent ID")]
    public string ParentId { get; set; } = group.ParentId.ToString();

    [Display("User ID")]
    public string UserId { get; set; } = group.UserId.ToString();

    [Display("Organization ID")]
    public string OrganizationId { get; set; } = group.OrganizationId.ToString();
}