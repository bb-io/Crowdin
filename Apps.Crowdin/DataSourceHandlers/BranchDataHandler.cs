using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Crowdin.Api;
using Crowdin.Api.Branches;
using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Models.Response;
using Newtonsoft.Json;
using RestSharp;
using Apps.Crowdin.Models.Response.Branch;

namespace Apps.Crowdin.DataSourceHandlers
{
    public class BranchDataHandler : AppInvocable, IAsyncDataSourceItemHandler
    {
        private readonly ProjectRequest _projectRequest;

        public BranchDataHandler(
            InvocationContext invocationContext,
            [ActionParameter] ProjectRequest projectRequest
        ) : base(invocationContext)
        {
            _projectRequest = projectRequest;
        }

        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(_projectRequest.ProjectId))
                throw new PluginMisconfigurationException("You should input Project ID first");

            var items = await Paginator.Paginate(async (lim, offset) =>
            {
                var request = new CrowdinRestRequest(
                    $"/projects/{_projectRequest.ProjectId}/branches?limit={lim}&offset={offset}",
                    Method.Get, Creds);
                var response = await RestClient.ExecuteAsync(request, cancellationToken: cancellationToken);
                return JsonConvert.DeserializeObject<ResponseList<DataResponse<BranchEntity>>>(response.Content!)!;
            });

            return items
                .Select(x => x.Data)
                .Where(x => context.SearchString == null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Name)
                .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
        }
    }
}
