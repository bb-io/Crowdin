using Blackbird.Applications.Sdk.Common;
using Crowdin.Api.Storage;

namespace Apps.Crowdin.Models.Entities;

public class StorageEntity
{
    [Display("ID")]
    public string Id { get; set; }

    [Display("File name")] public string FileName { get; set; }

    public StorageEntity(StorageResource storageResource)
    {
        Id = storageResource.Id.ToString();
        FileName = storageResource.FileName;
    }
}