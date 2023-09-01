using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.TranslationMemory;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.TranslationMemory;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Crowdin.Api.TranslationMemory;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TranslationMemoryActions : BaseInvocable
{
    private AuthenticationCredentialsProvider[] Creds =>
        InvocationContext.AuthenticationCredentialsProviders.ToArray();

    public TranslationMemoryActions(InvocationContext invocationContext) : base(invocationContext)
    {
    }
    
    [Action("List translation memories", Description = "List all translation memories")]
    public async Task<ListTranslationMemoriesResponse> ListTranslationMemories(
        [ActionParameter] ListTranslationMemoryRequest input)
    {
        var intUserId = IntParser.Parse(input.UserId, nameof(input.UserId));
        var intGroupId = IntParser.Parse(input.GroupId, nameof(input.GroupId));

        var client = new CrowdinClient(Creds);

        var items = await Paginator.Paginate((lim, offset)
            => client.TranslationMemory.ListTms(intUserId, intGroupId, lim, offset));

        var tms = items.Select(x => new TranslationMemoryEntity(x)).ToArray();
        return new(tms);
    }

    [Action("Get translation memory", Description = "Get specific translation memory")]
    public async Task<TranslationMemoryEntity> GetTranslationMemory(
        [ActionParameter] TranslationMemoryRequest tm)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));
        var client = new CrowdinClient(Creds);

        var response = await client.TranslationMemory.GetTm(intTmId!.Value);
        return new(response);
    }

    [Action("Add translation memory", Description = "Add new translation memory")]
    public async Task<TranslationMemoryEntity> AddTranslationMemory(
        [ActionParameter] AddTranslationMemoryRequest input)
    {
        var intGroupId = IntParser.Parse(input.GroupId, nameof(input.GroupId));
        var client = new CrowdinClient(Creds);

        var request = new AddTmRequest
        {
            Name = input.Name,
            LanguageId = input.LanguageId,
            GroupId = intGroupId
        };
        var response = await client.TranslationMemory.AddTm(request);

        return new(response);
    }

    [Action("Delete translation memory", Description = "Delete specific translation memory")]
    public Task DeleteTranslationMemory([ActionParameter] TranslationMemoryRequest tm)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));
        var client = new CrowdinClient(Creds);

        return client.TranslationMemory.DeleteTm(intTmId!.Value);
    }

    [Action("Export translation memory", Description = "Export specific translation memory")]
    public async Task<TmExportEntity> ExportTranslationMemory(
        [ActionParameter] TranslationMemoryRequest tm,
        [ActionParameter] ExportTranslationMemoryRequest input)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));

        var formatEnum =
            EnumParser.Parse<TmFileFormat>(input.Format, nameof(input.Format), EnumValues.TmFileFormat);

        var client = new CrowdinClient(Creds);

        var request = new ExportTmRequest
        {
            SourceLanguageId = input.SourceLanguageId,
            TargetLanguageId = input.TargetLanguageId,
            Format = formatEnum
        };
        var response = await client.TranslationMemory.ExportTm(intTmId!.Value, request);

        return new(response);
    }

    [Action("Download translation memory", Description = "Download specific translation memory")]
    public async Task<DownloadFileResponse> DownloadTranslationMemory(
        [ActionParameter] DownloadTranslationMemoryRequest input)
    {
        var intTmId = IntParser.Parse(input.TranslationMemoryId, nameof(input.TranslationMemoryId));
        var client = new CrowdinClient(Creds);

        var response = await client.TranslationMemory.DownloadTm(intTmId!.Value, input.ExportId);
        var fileContent = await FileDownloader.DownloadFileBytes(response.Url);
        return new(fileContent);
    }

    [Action("Add translation memory segment", Description = "Add new segment to the translation memory")]
    public async Task<TmSegmentRecordEntity> AddTmSegment(
        [ActionParameter] AddTranslationMemorySegmentRequest input)
    {
        var intTmId = IntParser.Parse(input.TranslationMemoryId, nameof(input.TranslationMemoryId));
        var client = new CrowdinClient(Creds);

        var request = new CreateTmSegmentRequest
        {
            Records = new List<TmSegmentRecordForm>
            {
                new()
                {
                    LanguageId = input.LanguageId,
                    Text = input.Text
                }
            }
        };

        var response = await client.TranslationMemory.CreateTmSegment(intTmId!.Value, request);
        return new(response.Records.First());
    }
}