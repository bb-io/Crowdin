using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Request.Team;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class TeamsTests :TestBase
    {
        [TestMethod]
        public async Task ListTeams_ReturnsList()
        {
            var action = new TeamActions(InvocationContext);

            var response = await action.ListTeams();
            foreach (var member in response.Teams)
            {
                Console.WriteLine($"{member.Name} - {member.Id}");
                Assert.IsNotNull(member);
            }
            Assert.IsNotNull(response);

        }

        [TestMethod]
        public async Task TeamMembers_ReturnsList()
        {
            var action = new TeamActions(InvocationContext);
            var teamId = new TeamRequest { TeamId = "1" };

            var response = await action.ListTeamMembers(teamId);
            foreach (var member in response.Members)
            {
                Console.WriteLine($"{member.FirstName} - {member.Id} - {member.LastName}");
                Assert.IsNotNull(member);
            }

        }
    }
}
