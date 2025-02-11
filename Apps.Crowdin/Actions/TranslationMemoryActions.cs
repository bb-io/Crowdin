using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.TranslationMemory;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.TranslationMemory;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Crowdin.Api.TranslationMemory;
using Apps.Crowdin.Invocables;

namespace Apps.Crowdin.Actions;

[ActionList]
public class TranslationMemoryActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AppInvocable(invocationContext)
{
    [Action("Search translation memories", Description = "List all translation memories")]
    public async Task<ListTranslationMemoriesResponse> ListTranslationMemories(
        [ActionParameter] ListTranslationMemoryRequest input)
    {
        var intUserId = IntParser.Parse(input.UserId, nameof(input.UserId));
        var intGroupId = IntParser.Parse(input.GroupId, nameof(input.GroupId));

        var items = await Paginator.Paginate((lim, offset)
            => ExceptionWrapper.ExecuteWithErrorHandling(() =>
                SdkClient.TranslationMemory.ListTms(intUserId, intGroupId, lim, offset)));

        var tms = items.Select(x => new TranslationMemoryEntity(x)).ToArray();
        return new(tms);
    }

    [Action("Get translation memory", Description = "Get specific translation memory")]
    public async Task<TranslationMemoryEntity> GetTranslationMemory(
        [ActionParameter] TranslationMemoryRequest tm)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));
        var response =
            await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
                await SdkClient.TranslationMemory.GetTm(intTmId!.Value));
        return new(response);
    }

    [Action("Add translation memory", Description = "Add new translation memory")]
    public async Task<TranslationMemoryEntity> AddTranslationMemory(
        [ActionParameter] AddTranslationMemoryRequest input)
    {
        var intGroupId = IntParser.Parse(input.GroupId, nameof(input.GroupId));
        var request = new AddTmRequest
        {
            Name = input.Name,
            LanguageId = input.LanguageId,
            GroupId = intGroupId
        };

        var response =
            await ExceptionWrapper.ExecuteWithErrorHandling(
                async () => await SdkClient.TranslationMemory.AddTm(request));
        return new(response);
    }

    [Action("Delete translation memory", Description = "Delete specific translation memory")]
    public async Task DeleteTranslationMemory([ActionParameter] TranslationMemoryRequest tm)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));
        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.TranslationMemory.DeleteTm(intTmId!.Value));
    }

    [Action("Export translation memory", Description = "Export specific translation memory")]
    public async Task<TmExportEntity> ExportTranslationMemory(
        [ActionParameter] TranslationMemoryRequest tm,
        [ActionParameter] ExportTranslationMemoryRequest input)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));

        var formatEnum =
            EnumParser.Parse<TmFileFormat>(input.Format, nameof(input.Format));

        var request = new ExportTmRequest
        {
            SourceLanguageId = input.SourceLanguageId,
            TargetLanguageId = input.TargetLanguageId,
            Format = formatEnum
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.TranslationMemory.ExportTm(intTmId!.Value, request));
        return new(response);
    }

    [Action("Download translation memory", Description = "Download specific translation memory")]
    public async Task<DownloadFileResponse> DownloadTranslationMemory(
        [ActionParameter] DownloadTranslationMemoryRequest input)
    {
        var intTmId = IntParser.Parse(input.TranslationMemoryId, nameof(input.TranslationMemoryId));
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.TranslationMemory.DownloadTm(intTmId!.Value, input.ExportId));

        var fileContent = await FileDownloader.DownloadFileBytes(response.Url);
        await FileOperationWrapper.ExecuteFileOperation(() => Task.CompletedTask, fileContent.FileStream, fileContent.Name);
        var file = await fileManagementClient.UploadAsync(fileContent.FileStream, fileContent.ContentType,
            fileContent.Name);
        return new(file);
    }

    [Action("Add translation memory segment", Description = "Add new segment to the translation memory")]
    public async Task<TmSegmentRecordEntity> AddTmSegment(
        [ActionParameter] AddTranslationMemorySegmentRequest input)
    {
        var intTmId = IntParser.Parse(input.TranslationMemoryId, nameof(input.TranslationMemoryId));

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

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.TranslationMemory.CreateTmSegment(intTmId!.Value, request));

        return new(response.Records.First());
    }
}