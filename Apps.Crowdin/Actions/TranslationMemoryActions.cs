using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.TranslationMemory;
using Apps.Crowdin.Models.Response.File;
using Apps.Crowdin.Models.Response.TranslationMemory;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using Blackbird.Applications.Sdk.Utils.Utilities;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Crowdin.Api.TranslationMemory;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Actions;

[ActionList("Translation memory")]
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

        if (!int.TryParse(input.TranslationMemoryId, out var intTmId))
        {
            throw new PluginMisconfigurationException(
                $"Invalid Translation Memory ID: {input.TranslationMemoryId} must be a numeric value. Please check the input.");
        }

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.TranslationMemory.DownloadTm(intTmId, input.ExportId));

        var fileContent = await FileDownloader.DownloadFileBytes(response.Url);
        await FileOperationWrapper.ExecuteFileOperation(
            () => Task.CompletedTask, fileContent.FileStream, fileContent.Name);

        var file = await fileManagementClient.UploadAsync(
            fileContent.FileStream, fileContent.ContentType, fileContent.Name);

        return new DownloadFileResponse(file);
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

    [Action("Import translation memory", Description = "Import translation memory from a file into an existing TM")]
    public async Task<TmImportEntity> ImportTranslationMemory(
       [ActionParameter] TranslationMemoryRequest tm,
       [ActionParameter] ImportTranslationMemoryRequest input)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));

        if (intTmId is null)
        {
            throw new PluginMisconfigurationException(
                $"Invalid Translation Memory ID: {tm.TranslationMemoryId} must be a numeric value. Please check the input.");
        }

        await using var fileStream = await fileManagementClient.DownloadAsync(input.File);
        var storage = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Storage.AddStorage(fileStream, input.File.Name));

        var request = new ImportTmRequest
        {
            StorageId = storage.Id
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.TranslationMemory.ImportTm(intTmId.Value, request));

        return new TmImportEntity(response);
    }

    [Action("Search TM segments", Description = "Search segments in a translation memory")]
    public async Task<IEnumerable<TmSegmentEntity>> SearchTmSegments(
       [ActionParameter] TranslationMemoryRequest tm,
       [ActionParameter] SearchTmSegmentsRequest input)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));

        if (intTmId is null)
        {
            throw new PluginMisconfigurationException(
                $"Invalid Translation Memory ID: {tm.TranslationMemoryId} must be a numeric value. Please check the input.");
        }

        var segments = await Paginator.Paginate((limit, offset) =>
            ExceptionWrapper.ExecuteWithErrorHandling(() =>
                SdkClient.TranslationMemory.ListTmSegments(
                    intTmId.Value,
                    limit,
                    offset)));

        var result = segments
            .Select(seg =>
            {
                var segRecords = seg.Records.AsEnumerable();

                if (!string.IsNullOrWhiteSpace(input.LanguageId))
                {
                    segRecords = segRecords.Where(r =>
                        string.Equals(r.LanguageId, input.LanguageId,
                            StringComparison.OrdinalIgnoreCase));
                }

                var recordEntities = segRecords
                    .Select(r => new TmSegmentRecordEntity(r))
                    .ToArray();

                return new TmSegmentEntity
                {
                    Id = seg.Id.ToString(),
                    Records = recordEntities
                };
            })
            .Where(s => s.Records.Any())
            .ToArray();

        return result;
    }

    [Action("Get TM segment", Description = "Get specific translation memory segment")]
    public async Task<TmSegmentEntity> GetTmSegment(
       [ActionParameter] TranslationMemoryRequest tm,
       [ActionParameter] TmSegmentRequest segment)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));
        if (intTmId is null)
        {
            throw new PluginMisconfigurationException(
                $"Invalid Translation Memory ID: {tm.TranslationMemoryId} must be a numeric value. Please check the input.");
        }

        var intSegmentId = IntParser.Parse(segment.SegmentId, nameof(segment.SegmentId));
        if (intSegmentId is null)
        {
            throw new PluginMisconfigurationException(
                $"Invalid Segment ID: {segment.SegmentId} must be a numeric value. Please check the input.");
        }

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.TranslationMemory.GetTmSegment(intTmId.Value, intSegmentId.Value));
        return new TmSegmentEntity(response);
    }

    [Action("Delete TM segment", Description = "Delete specific translation memory segment")]
    public async Task DeleteTmSegment(
        [ActionParameter] TranslationMemoryRequest tm,
        [ActionParameter] TmSegmentRequest segment)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));
        if (intTmId is null)
        {
            throw new PluginMisconfigurationException(
                $"Invalid Translation Memory ID: {tm.TranslationMemoryId} must be a numeric value. Please check the input.");
        }

        var intSegmentId = IntParser.Parse(segment.SegmentId, nameof(segment.SegmentId));
        if (intSegmentId is null)
        {
            throw new PluginMisconfigurationException(
                $"Invalid Segment ID: {segment.SegmentId} must be a numeric value. Please check the input.");
        }

        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.TranslationMemory.DeleteTmSegment(intTmId.Value, intSegmentId.Value));
    }

    [Action("Edit TM segment", Description = "Edit text of a record inside a translation memory segment")]
    public async Task<TmSegmentEntity> EditTmSegment(
      [ActionParameter] TranslationMemoryRequest tm,
      [ActionParameter] TmSegmentRequest segment,
      [ActionParameter] EditTmSegmentRequest input)
    {
        var intTmId = IntParser.Parse(tm.TranslationMemoryId, nameof(tm.TranslationMemoryId));
        var intSegmentId = IntParser.Parse(segment.SegmentId, nameof(segment.SegmentId));

        if (intTmId is null)
            throw new PluginMisconfigurationException(
                $"Invalid Translation Memory ID: {tm.TranslationMemoryId} must be a numeric value. Please check the input.");

        if (intSegmentId is null)
            throw new PluginMisconfigurationException(
                $"Invalid Segment ID: {segment.SegmentId} must be a numeric value. Please check the input.");

        var op = input.Operation?.Trim().ToLowerInvariant();
        object body;

        switch (op)
        {
            case "add":
                {
                    if (string.IsNullOrWhiteSpace(input.LanguageId))
                        throw new PluginMisconfigurationException(
                            "Language ID is required for 'add' operation.");

                    if (string.IsNullOrWhiteSpace(input.Text))
                        throw new PluginMisconfigurationException(
                            "Text is required for 'add' operation.");

                    body = new[]
                    {
                    new
                    {
                        op = "add",
                        path = "/records/-",
                        value = new
                        {
                            text = input.Text,
                            languageId = input.LanguageId
                        }
                    }
                };
                    break;
                }

            case "replace":
                {
                    var intRecordId = IntParser.Parse(input.RecordId, nameof(input.RecordId));
                    if (intRecordId is null)
                        throw new PluginMisconfigurationException(
                            "Record ID is required for 'replace' operation.");

                    if (string.IsNullOrWhiteSpace(input.Text))
                        throw new PluginMisconfigurationException(
                            "Text is required for 'replace' operation.");

                    body = new[]
                    {
                    new
                    {
                        op = "replace",
                        path = $"/records/{intRecordId.Value}/text",
                        value = input.Text
                    }
                };
                    break;
                }

            case "remove":
                {
                    var intRecordId = IntParser.Parse(input.RecordId, nameof(input.RecordId));
                    if (intRecordId is null)
                        throw new PluginMisconfigurationException(
                            "Record ID is required for 'remove' operation.");

                    body = new[]
                    {
                    new
                    {
                        op = "remove",
                        path = $"/records/{intRecordId.Value}"
                    }
                };
                    break;
                }

            default:
                throw new PluginMisconfigurationException(
                    $"Unsupported operation '{input.Operation}'. Allowed values: add, replace, remove.");
        }

        var plan = InvocationContext.AuthenticationCredentialsProviders.GetCrowdinPlan();

        BlackBirdRestClient restClient = plan == Plans.Enterprise
            ? new CrowdinEnterpriseRestClient(InvocationContext.AuthenticationCredentialsProviders)
            : new CrowdinRestClient();

        var request = new CrowdinRestRequest(
            $"/tms/{intTmId.Value}/segments/{intSegmentId.Value}",
            Method.Patch,
            InvocationContext.AuthenticationCredentialsProviders);

        request.AddJsonBody(body);

        var response = await restClient.ExecuteWithErrorHandling(request);

        var segmentDto = JsonConvert.DeserializeObject<TmSegmentResponseDto>(response.Content ?? string.Empty);
        if (segmentDto?.Data == null)
        {
            throw new PluginApplicationException(
                "Failed to parse TM segment response from Crowdin.");
        }

        var recordEntities = segmentDto.Data.Records
            .Select(r => new TmSegmentRecordEntity
            {
                Id = r.Id.ToString(),
                LanguageId = r.LanguageId,
                Text = r.Text
            })
            .ToArray();

        return new TmSegmentEntity
        {
            Id = segmentDto.Data.Id.ToString(),
            Records = recordEntities
        };
    }
}