using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.SourceString;
using Apps.Crowdin.Models.Response.SourceString;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.SourceStrings;

namespace Apps.Crowdin.Actions;

[ActionList]
public class SourceStringAction(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("Search strings", Description = "List all project source strings")]
    public async Task<ListStringsResponse> ListStrings([ActionParameter] ListStringsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        
        var items = await Paginator.Paginate((lim, offset)
            =>
        {
            var request = new StringsListParams
            {
                Limit = lim,
                Offset = offset,
                FileId = IntParser.Parse(input.FileId, nameof(input.FileId)),
                BranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId)),
                DirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId)),
                LabelIds = input.LabelIds,
                Filter = input.Filter,
                CroQL = input.CroQl,
                Scope = EnumParser.Parse<StringScope>(input.Scope, nameof(input.Scope)),
                DenormalizePlaceholders = input.DenormalizePlaceholders is true ? 1 : 0,
            };
            return ExceptionWrapper.ExecuteWithErrorHandling(() => SdkClient.SourceStrings.ListStrings(intProjectId!.Value, request));
        });

        var strings = items.Select(x => new SourceStringEntity(x)).ToArray();
        return new(strings);
    }

    [Action("Find source string", Description = "Return first matching source strings")]
    public async Task<SourceStringEntity> FindString([ActionParameter] ListStringsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));

        var request = new StringsListParams
        {
            Limit = 1,
            Offset = 0,
            FileId = IntParser.Parse(input.FileId, nameof(input.FileId)),
            BranchId = IntParser.Parse(input.BranchId, nameof(input.BranchId)),
            DirectoryId = IntParser.Parse(input.DirectoryId, nameof(input.DirectoryId)),
            LabelIds = input.LabelIds,
            Filter = input.Filter,
            CroQL = input.CroQl,
            Scope = EnumParser.Parse<StringScope>(input.Scope, nameof(input.Scope)),
            DenormalizePlaceholders = input.DenormalizePlaceholders is true ? 1 : 0,
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.SourceStrings
            .ListStrings(intProjectId!.Value, request));

        return new(response.Data.FirstOrDefault());
    }


    [Action("Get source string", Description = "Get specific source string")]
    public async Task<SourceStringEntity> GetString([ActionParameter] GetSourceStringRequest input)
    {
        if (!int.TryParse(input.ProjectId, out var intProjectId))
            throw new PluginMisconfigurationException($"Invalid Project ID: {input.ProjectId} must be a numeric value. Please check the input project ID");

        if (!int.TryParse(input.StringId, out var intStringId))
            throw new PluginMisconfigurationException($"Invalid String ID: {input.StringId} must be a numeric value. Please check the input string ID");

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.SourceStrings
        .GetString(intProjectId, intStringId, input.DenormalizePlaceholders ?? false));

        return new(response);
    }

    [Action("Add source string", Description = "Add new source string")]
    public async Task<SourceStringEntity> AddString([ActionParameter] AddSourceStringRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var request = new AddStringRequest
        {
            Text = input.Text,
            Identifier = input.Identifier,
            FileId = IntParser.Parse(input.FileId, nameof(input.FileId)),
            Context = input.Context,
            IsHidden = input.IsHidden,
            MaxLength = input.MaxLength,
            LabelIds = input.LabelIds?.Select(labelId => IntParser.Parse(labelId, nameof(labelId))!.Value).ToList()
        };
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.SourceStrings
            .AddString(intProjectId!.Value, request));
        
        return new(response);
    }

    [Action("Delete source string", Description = "Delete specific source string")]
    public async Task DeleteString([ActionParameter] DeleteSourceStringRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));
        
        await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.SourceStrings.DeleteString(intProjectId!.Value, intStringId!.Value));
    }
}