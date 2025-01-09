using Apps.Crowdin.Api;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Dynamic;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Crowdin.Api;
using Crowdin.Api.Translations;

namespace Apps.Crowdin.DataSourceHandlers;

public class PreTranslationDataSource(
    InvocationContext invocationContext,
    [ActionParameter] ProjectRequest projectRequest) : AppInvocable(invocationContext), IAsyncDataSourceItemHandler
{
    public async Task<IEnumerable<DataSourceItem>> GetDataAsync(DataSourceContext context, CancellationToken cancellationToken)
    {
        if (string.IsNullOrEmpty(projectRequest.ProjectId))
        {
            throw new PluginMisconfigurationException("Please, provide Project ID first");
        }

        var items = await Paginator.Paginate((lim, offset)
            => SdkClient.Translations.ListPreTranslations(int.Parse(projectRequest.ProjectId), lim, offset));

        return items
            .Where(x => x.Status != BuildStatus.Failed && x.Status != BuildStatus.Canceled &&
                        x.Status != BuildStatus.Finished)
            .Where(x => context.SearchString == null ||
                        GetReadableName(x).Contains(context.SearchString, StringComparison.OrdinalIgnoreCase))
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new DataSourceItem(x.Identifier, GetReadableName(x)));
    }

    private string GetReadableName(PreTranslation preTranslation) =>
        $"[{preTranslation.Status}] Progress: {preTranslation.Progress}";
}