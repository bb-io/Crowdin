using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Crowdin.DataSourceHandlers;

public class WorkflowStepDataHandler(
    InvocationContext invocationContext, 
    [ActionParameter] ProjectRequest projectRequest) : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(projectRequest.ProjectId))
        {
            throw new PluginMisconfigurationException("Please, provide Project ID first");
        }

        var result = await SdkClient.Workflows.ListWorkflowSteps(int.Parse(projectRequest.ProjectId));
        return result.Data.Select(x => new DataSourceItem(x.Id.ToString(), x.Title));
    }
}
