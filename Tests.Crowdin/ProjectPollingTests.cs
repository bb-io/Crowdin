using Apps.Crowdin.Polling;
using Apps.Crowdin.Polling.Models;
using Apps.Crowdin.Polling.Models.Requests;
using Blackbird.Applications.Sdk.Common.Polling;
using Tests.Crowdin.Base;

namespace Tests.Crowdin;

[TestClass]
public class ProjectPollingTests : TestBase
{
    [TestMethod]
    public async Task OnProjectAssignedToVendor_ReturnsExternalProjects()
    {
        // Arrange
        var polling = new ProjectPollingList(InvocationContext);
        var projectsMemory = new ProjectsPollingMemory { };
        var pollingRequest = new PollingEventRequest<ProjectsPollingMemory>() { Memory = projectsMemory };
        var input = new OnProjectAssignedToVendorRequest { };

        // Act
        var result = await polling.OnProjectAssignedToVendor(pollingRequest, input);

        // Assert
        PrintJsonResult(result);
        Assert.IsNotNull(result);
    }
}