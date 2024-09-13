using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request;
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
    : BaseInvocable(invocationContext), IAsyncDataSourceHandler
{
    private AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(assigneesRequest.ProjectId))
            throw new("You should input Project ID first");

        var client = new CrowdinRestClient();

        var items = await Paginator.Paginate(async (lim, offset)
            =>
        {
            var request =
                new CrowdinRestRequest(
                    $"/projects/{assigneesRequest.ProjectId}/members?limit={lim}&offset={offset}",
                    Method.Get, Creds);
            var response = await client.ExecuteAsync(request, cancellationToken: cancellationToken);
            return JsonConvert.DeserializeObject<ResponseList<DataResponse<AssigneeEntity>>>(response.Content);
        });

        return items
            .Select(x => x.Data)
            .Where(x => context.SearchString == null ||
                        x.FullName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.GivenAccessAt)
            .Take(20)
            .ToDictionary(x => x.Id.ToString(), x => x.FullName);
    }
}