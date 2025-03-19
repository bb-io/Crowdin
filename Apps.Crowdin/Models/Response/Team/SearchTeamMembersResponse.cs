using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Teams;

namespace Apps.Crowdin.Models.Response.Team
{
    public class SearchTeamMembersResponse
    {
        [Display("Team members")]
        public IEnumerable<TeamMember> Members { get; set; }

        [Display("Members IDs")]
        public IEnumerable<string> Ids { get; set; }
    }
}
