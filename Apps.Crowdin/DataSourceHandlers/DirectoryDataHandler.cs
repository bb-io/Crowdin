using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Crowdin.Api.SourceFiles;

namespace Apps.Crowdin.DataSourceHandlers;

public class DirectoryDataHandler(InvocationContext invocationContext, 
    [ActionParameter] ProjectRequest projectRequest)
    : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(projectRequest.ProjectId))
        {
            throw new PluginMisconfigurationException("You should specify Project ID first");
        }
        
        var items = await Paginator.Paginate((lim, offset)
            =>
        {
            var request = new DirectoriesListParams
            {
                Limit = lim,
                Offset = offset
            };
            
            return SdkClient.SourceFiles.ListDirectories(int.Parse(projectRequest.ProjectId), request);
        });
        
        return items
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
    }
}