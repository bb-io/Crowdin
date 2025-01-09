using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.Users;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Crowdin.Api;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.DataSourceHandlers;

public class AiPromptIdDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] ProjectRequest project,
    [ActionParameter] UserRequest user) : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(project.ProjectId) || string.IsNullOrEmpty(user.UserId))
            throw new("You should input Project ID and User ID first");
        
        var items = await Paginator.Paginate(async (lim, offset)
            =>
        {
            var request =
                new CrowdinRestRequest(
                    $"/users/{user.UserId}/ai/prompts?limit={lim}&offset={offset}",
                    Method.Get, Creds);
            request.AddQueryParameter("projectId", project.ProjectId);
            request.AddQueryParameter("action", "pre_translate");
            var response = await RestClient.ExecuteAsync(request, cancellationToken: cancellationToken);
            return JsonConvert.DeserializeObject<ResponseList<DataResponse<AiPromptEntity>>>(response.Content);
        });

        return items
            .Select(x => x.Data)
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
    }
}