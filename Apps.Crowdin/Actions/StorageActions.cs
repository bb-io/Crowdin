using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Storage;
using Apps.Crowdin.Models.Response.Storage;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.SDK.Extensions.FileManagement.Interfaces;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Crowdin.Actions;

[ActionList("Storages")]
public class StorageActions(InvocationContext invocationContext, IFileManagementClient fileManagementClient)
    : AppInvocable(invocationContext)
{
    [Action("Search storages", Description = "List all storages")]
    public async Task<ListStoragesResponse> ListStorages()
    {
        var responseItems = await Paginator.Paginate((lim, offset)
            => ExceptionWrapper.ExecuteWithErrorHandling(() => SdkClient.Storage.ListStorages(lim, offset)));

        var storages = responseItems.Select(x => new StorageEntity(x)).ToArray();
        return new(storages);
    }

    [Action("Get storage", Description = "Get specific storage")]
    public async Task<StorageEntity> GetStorage([ActionParameter] StorageRequest storage)
    {
        var intStorageId = IntParser.Parse(storage.StorageId, nameof(storage.StorageId));
        var response =
            await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
                await SdkClient.Storage.GetStorage(intStorageId!.Value));
        return new(response);
    }

    [Action("Add storage", Description = "Add new storage")]
    public async Task<StorageEntity> AddStorage([ActionParameter] AddStorageRequest input)
    {
        var fileName = input.FileName ?? input.File.Name;
        if (!IsOnlyAscii(fileName))
        {
            throw new PluginMisconfigurationException(
                $"The file name '{fileName}' contains non-ASCII characters. " +
                "Crowdin API requires ASCII-only characters. Please rename the file and try again.");
        }

        var stream = await FileOperationWrapper.ExecuteFileDownloadOperation(() => fileManagementClient.DownloadAsync(input.File), input.File.Name);
        var memoryStream = new MemoryStream();
        await stream.CopyToAsync(memoryStream);

        memoryStream.Position = 0;

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => await SdkClient.Storage
            .AddStorage(memoryStream, input.FileName ?? input.File.Name));
        return new(response);
    }

    [Action("Delete storage", Description = "Delete specific storage")]
    public async Task DeleteStorage([ActionParameter] StorageRequest storage)
    {
        var intStorageId = IntParser.Parse(storage.StorageId, nameof(storage.StorageId));
        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Storage.DeleteStorage(intStorageId!.Value));
    }

    private bool IsOnlyAscii(string input)
    {
        return input.All(c => c <= 127);
    }
}