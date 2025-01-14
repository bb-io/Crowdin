using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Team;
using Apps.Crowdin.Models.Response.Team;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api;
using Crowdin.Api.Teams;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TeamActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("[Enterprise] Search teams", Description = "List all teams")]
    public async Task<ListTeamsResponse> ListTeams()
    {
        CheckAccessToEnterpriseAction();
        var response = await Paginator.Paginate(ListTeamsAsync);

        var teams = response.Select(x => new TeamEntity(x)).ToArray();
        return new(teams);
    }

    [Action("[Enterprise] Get team", Description = "Get specific team")]
    public async Task<TeamEntity> GetTeam(
        [ActionParameter] TeamRequest team)
    {
        CheckAccessToEnterpriseAction();
        var intTeamId = IntParser.Parse(team.TeamId, nameof(team.TeamId))!.Value;
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.Teams.GetTeam(intTeamId));
        return new(response);
    }

    [Action("[Enterprise] Add team", Description = "Add a new team")]
    public async Task<TeamEntity> AddTeam(
        [ActionParameter] [Display("Name")] string name)
    {
        CheckAccessToEnterpriseAction();
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.Teams.AddTeam(new()
        {
            Name = name
        }));
        
        return new(response);
    }

    [Action("[Enterprise] Delete team", Description = "Delete specific team")]
    public async Task DeleteTeam(
        [ActionParameter] TeamRequest team)
    {
        CheckAccessToEnterpriseAction();
        await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.Teams.DeleteTeam(IntParser.Parse(team.TeamId, nameof(team.TeamId))!.Value));
    }
    
    private Task<ResponseList<Team>> ListTeamsAsync(int limit = 25, int offset = 0)
    {
        return ExceptionWrapper.ExecuteWithErrorHandling(() => SdkClient.Teams.ListTeams(limit, offset));
    }
}