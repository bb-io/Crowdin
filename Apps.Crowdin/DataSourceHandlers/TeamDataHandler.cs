using Apps.Crowdin.Invocables;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Crowdin.Api;
using Crowdin.Api.Teams;

namespace Apps.Crowdin.DataSourceHandlers;

public class TeamDataHandler(InvocationContext invocationContext)
    : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
{   
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {        
        var items = await Paginator.Paginate(ListTeamsAsync);

        return items
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
    }
    
    private Task<ResponseList<Team>> ListTeamsAsync(int limit = 25, int offset = 0)
    {
        return SdkClient.Teams.ListTeams(limit, offset);
    }
}