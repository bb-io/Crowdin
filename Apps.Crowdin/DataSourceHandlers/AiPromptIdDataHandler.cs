using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request;
using Apps.Crowdin.Models.Request.Project;
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

public class AiPromptIdDataHandler(
    InvocationContext invocationContext,
    [ActionParameter] ProjectRequest project, 
    [ActionParameter][Display("User ID")] string? UserId) 

    : BaseInvocable(invocationContext), IAsyncDataSourceHandler
{
    private AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    public async Task<Dictionary<string, string>> GetDataAsync(DataSourceContext context,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(project.ProjectId) || string.IsNullOrEmpty(UserId))
            throw new("You should input Project ID and User ID first");

        var client = new CrowdinRestClient();

        var items = await Paginator.Paginate(async (lim, offset)
            =>
        {
            var request =
                new CrowdinRestRequest(
                    $"/users/{UserId}/ai/prompts?limit={lim}&offset={offset}",
                    Method.Get, Creds);
            request.AddQueryParameter("projectId", project.ProjectId);
            request.AddQueryParameter("action", "pre_translate");
            var response = await client.ExecuteAsync(request, cancellationToken: cancellationToken);
            return JsonConvert.DeserializeObject<ResponseList<DataResponse<AiPromptEntity>>>(response.Content);
        });

        return items
            .Select(x => x.Data)
            .Where(x => context.SearchString == null ||
                        x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .Take(20)
            .ToDictionary(x => x.Id.ToString(), x => x.Name);
    }
}