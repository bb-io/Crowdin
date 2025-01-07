using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.Team;

public record ListTeamsResponse(TeamEntity[] Teams);