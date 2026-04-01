using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Filter;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Request.SourceString;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Models.Response.SourceString;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api;
using Crowdin.Api.SourceStrings;
using RestSharp;

namespace Apps.Crowdin.Actions;

[ActionList("Source strings")]
public class SourceStringAction(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("Search strings", Description = "List all project source strings")]
    public async Task<ListStringsResponse> ListStrings(
        [ActionParameter] ProjectRequest projectRequest,
        [ActionParameter] ListStringsRequest input,
        [ActionParameter] FieldsFilterRequest fieldsFilter)
    {
        fieldsFilter.Validate();

        var strings = await GetSourceStrings(projectRequest, input);        
        strings = strings.ApplyFieldsFilter(x => x.Fields, fieldsFilter);

        return new(strings.ToList());
    }

    [Action("Find source string", Description = "Return first matching source strings")]
    public async Task<SourceStringEntity?> FindString(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] ListStringsRequest input,
        [ActionParameter] FieldsFilterRequest fieldsFilter)
    {
        fieldsFilter.Validate();

        var strings = await GetSourceStrings(project, input);
        strings = strings.ApplyFieldsFilter(x => x.Fields, fieldsFilter);

        return strings.FirstOrDefault();
    }

    [Action("Get source string", Description = "Get specific source string")]
    public async Task<SourceStringEntity> GetString(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] GetSourceStringRequest input)
    {
        var request = new CrowdinRestRequest($"/projects/{project.ProjectId}/strings/{input.StringId}", Method.Get, Creds);
        if (input.DenormalizePlaceholders == true)
            request.AddQueryParameter("denormalizePlaceholders", 1);

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(
            () => RestClient.ExecuteWithErrorHandling<DataResponse<SourceStringEntity>>(request)
        );

        return response.Data;
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
            LabelIds = input.LabelIds?.Select(labelId => LongParser.Parse(labelId, nameof(labelId))!.Value).ToList()
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

    private async Task<IEnumerable<SourceStringEntity>> GetSourceStrings(
        ProjectRequest projectRequest,
        ListStringsRequest listStringsRequest)
    {
        var items = await Paginator.Paginate(async (lim, offset) =>
        {
            var request = new CrowdinRestRequest($"/projects/{projectRequest.ProjectId}/strings", Method.Get, Creds);

            if (!string.IsNullOrEmpty(listStringsRequest.FileId))
                request.AddQueryParameter("fileId", listStringsRequest.FileId);

            if (!string.IsNullOrEmpty(listStringsRequest.BranchId))
                request.AddQueryParameter("branchId", listStringsRequest.BranchId);

            if (!string.IsNullOrEmpty(listStringsRequest.DirectoryId))
                request.AddQueryParameter("directoryId", listStringsRequest.DirectoryId);

            if (!string.IsNullOrEmpty(listStringsRequest.LabelIds))
                request.AddQueryParameter("labelIds", listStringsRequest.LabelIds);

            if (!string.IsNullOrEmpty(listStringsRequest.CroQl))
                request.AddQueryParameter("croql", listStringsRequest.CroQl);

            if (!string.IsNullOrEmpty(listStringsRequest.Scope))
                request.AddQueryParameter("scope", listStringsRequest.Scope);

            if (listStringsRequest.DenormalizePlaceholders == true)
                request.AddQueryParameter("denormalizePlaceholders", 1);

            request.AddQueryParameter("limit", lim);
            request.AddQueryParameter("offset", offset);

            return await RestClient.ExecuteWithErrorHandling<ResponseList<DataResponse<SourceStringEntity>>>(request);
        });

        return items.Select(x => x.Data);
    }
}