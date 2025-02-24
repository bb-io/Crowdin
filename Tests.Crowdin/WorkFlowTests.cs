using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.Actions;
using Apps.Crowdin.Models.Request.Project;
using Tests.Crowdin.Base;

namespace Tests.Crowdin
{
    [TestClass]
    public class WorkFlowTests: TestBase
    {
        [TestMethod]
        public async Task ListWorkflowSteps_ReturnsSuccess()
        {
            var projectRequest = new ProjectRequest { ProjectId = "19" };
            var action = new WorkflowActions(InvocationContext);
            var result = await action.ListWorkflowSteps(projectRequest);

            foreach (var step in result.WorkflowSteps)
            {
                Console.WriteLine($"Workflow steps: {step.Id}");
                Assert.IsNotNull(result);
            }

        }
    }
}
