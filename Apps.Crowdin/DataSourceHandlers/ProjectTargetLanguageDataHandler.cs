using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Project;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.DataSourceHandlers
{
    public class ProjectTargetLanguageDataHandler(InvocationContext invocationContext, [ActionParameter] ProjectRequest project)
    : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
    {
        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {

            var request = new CrowdinRestRequest($"/projects/{project.ProjectId}", Method.Get, Creds);
            var response = await RestClient.ExecuteAsync<ProjectTargetLangResponse>(request, cancellationToken: cancellationToken);

            var targetIds = response?.Data?.Data.TargetLanguageIds;
            if (targetIds == null || targetIds.Length == 0)
            {
                return Array.Empty<DataSourceItem>();
            }

            var items = targetIds.Select(id => new DataSourceItem(id, id));
            if (!string.IsNullOrEmpty(context.SearchString))
            {
                items = items.Where(item => item.DisplayName.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase));
            }

            return items.OrderBy(item => item.DisplayName);
        }
    }

    internal class ProjectTargetLangResponse
    {
        [JsonProperty("data")]
        public ProjectDto Data { get; set; }
    }

    internal class ProjectDto
    {
        [JsonProperty("targetLanguageIds")]
        public string[] TargetLanguageIds { get; set; }
    }

}