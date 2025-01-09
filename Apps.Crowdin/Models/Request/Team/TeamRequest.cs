using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Team;

public class TeamRequest
{
    [Display("Team ID"), DataSource(typeof(TeamDataHandler))]
    public string TeamId { get; set; } = string.Empty;
}