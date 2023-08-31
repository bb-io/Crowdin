﻿using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.File;

public class AddNewFileRequest
{
    public string Name { get; set; }

    public Blackbird.Applications.Sdk.Common.Files.File? File { get; set; }
    
    [Display("Storage")]
    [DataSource(typeof(StorageDataHandler))]
    public string? StorageId { get; set; }
    
    [Display("Branch ID")]
    public string? BranchId { get; set; }

    [Display("Directory ID")]
    public string? DirectoryId { get; set; }

    public string? Title { get; set; }

    [Display("Excluded target languages")]
    public IEnumerable<string>? ExcludedTargetLanguages { get; set; }

    [Display("Attach label IDs")]
    public IEnumerable<int>? AttachLabelIds { get; set; }
}