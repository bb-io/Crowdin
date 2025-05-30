using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Users;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Crowdin.Api;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.DataSourceHandlers;

public class ProjectMemberDataSourceHandler(
    InvocationContext invocationContext,
    [ActionParameter] AssigneesRequest assigneesRequest)
    : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(assigneesRequest.ProjectId))
            throw new("You should input Project ID first");
        
        var items = await Paginator.Paginate(async (lim, offset)
            =>
        {
            var request =
                new CrowdinRestRequest(
                    $"/projects/{assigneesRequest.ProjectId}/members?limit={lim}&offset={offset}",
                    Method.Get, Creds);
            var response = await RestClient.ExecuteAsync(request, cancellationToken: cancellationToken);
            return JsonConvert.DeserializeObject<ResponseList<DataResponse<AssigneeEntity>>>(response.Content!)!;
        });

        return items
            .Select(x => x.Data)
            .Where(x => context.SearchString == null ||
                        x.FullName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.GivenAccessAt)
            .Select(x => new DataSourceItem(x.Id.ToString(), x.FullName));
    }
}