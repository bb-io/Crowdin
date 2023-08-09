using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Storage;
using Apps.Crowdin.Models.Response.Storage;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Parsers;

namespace Apps.Crowdin.Actions;

[ActionList]
public class StorageActions
{
    [Action("List storages", Description = "List all storages")]
    public async Task<ListStoragesResponse> ListStorages(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var client = new CrowdinClient(creds);

        var responseItems = await Paginator.Paginate((lim, offset)
            => client.Storage.ListStorages(lim, offset));

        var storages = responseItems.Select(x => new StorageEntity(x)).ToArray();
        return new(storages);
    }

    [Action("Get storage", Description = "Get specific storage")]
    public async Task<StorageEntity> GetStorage(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] StorageRequest storage)
    {
        var intStorageId = IntParser.Parse(storage.StorageId, nameof(storage.StorageId));
        var client = new CrowdinClient(creds);

        var response = await client.Storage.GetStorage(intStorageId!.Value);
        return new(response);
    }
    
    [Action("Add storage", Description = "Add new storage")]
    public async Task<StorageEntity> AddStorage(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] AddStorageRequest input)
    {
        var client = new CrowdinClient(creds);

        var response = await client.Storage
            .AddStorage(new MemoryStream(input.File), input.FileName);
        
        return new(response);
    }
    
    [Action("Delete storage", Description = "Delete specific storage")]
    public Task DeleteStorage(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] StorageRequest storage)
    {
        var intStorageId = IntParser.Parse(storage.StorageId, nameof(storage.StorageId));
        var client = new CrowdinClient(creds);

        return client.Storage.DeleteStorage(intStorageId!.Value);
    }
}