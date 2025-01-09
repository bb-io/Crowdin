using Apps.Crowdin.Invocables;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;

namespace Apps.Crowdin.DataSourceHandlers;

public class ProjectGroupDataHandler(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        var items = await Paginator.Paginate((lim, offset)
            => SdkClient.ProjectsGroups.ListGroups(null, lim, offset));

        return items
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
    }
}