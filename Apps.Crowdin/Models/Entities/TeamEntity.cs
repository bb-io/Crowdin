using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Teams;

namespace Apps.Crowdin.Models.Entities;

public class TeamEntity(Team team)
{
    [Display("Team ID")]
    public string Id { get; set; } = team.Id.ToString();

    public string Name { get; set; } = team.Name;

    [Display("Total members")]
    public int TotalMembers { get; set; } = team.TotalMembers;

    [Display("Created at")]
    public DateTime CreatedAt { get; set; } = team.CreatedAt.UtcDateTime;

    [Display("Updated at")]
    public DateTime? UpdatedAt { get; set; } = team.CreatedAt.UtcDateTime;
}