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
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.SourceFiles;

namespace Apps.Crowdin.DataSourceHandlers
{
    public class FileDataHandler(
      InvocationContext invocationContext,
      [ActionParameter] ProjectRequest projectRequest) : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
    {
        public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
        {
            if (string.IsNullOrEmpty(projectRequest.ProjectId))
            {
                throw new PluginMisconfigurationException("Please, provide Project ID first");
            }

            var intProjectId = IntParser.Parse(projectRequest.ProjectId, nameof(projectRequest.ProjectId));

            var items = await Paginator.Paginate((lim, offset) =>
            {
                var request = new FilesListParams
                {
                    Filter = context.SearchString,
                    Limit = lim,
                    Offset = offset
                };

                return ExceptionWrapper.ExecuteWithErrorHandling(() =>
                    SdkClient.SourceFiles.ListFiles<FileCollectionResource>(intProjectId!.Value, request));
            });

            return items
                .Where(x => context.SearchString == null ||
                            x.Name.Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
                .OrderBy(x => x.Name)
                .Select(x => new DataSourceItem(x.Id.ToString(), x.Name));
        }
    }
}
