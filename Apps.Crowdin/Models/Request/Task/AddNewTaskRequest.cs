﻿using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Task;

public class AddNewTaskRequest
{
    public string Title { get; set; }

    [Display("Language ID")]
    [DataSource(typeof(LanguagesDataHandler))]
    public string LanguageId { get; set; }

    [Display("File IDs")]
    public IEnumerable<string> FileIds { get; set; }

    [StaticDataSource(typeof(TaskTypeHandler))]
    public string Type { get; set; }

    [StaticDataSource(typeof(TaskStatusHandler))]
    public string? Status { get; set; }

    public string? Description { get; set; }
    
    [StaticDataSource(typeof(VendorDataHandler))]
    public string? Vendor { get; set; }

    [Display("Split files")]
    public bool? SplitFiles { get; set; }

    [Display("Skip assigned strings")]
    public bool? SkipAssignedStrings { get; set; }

    [Display("Skip untranslated strings")]
    public bool? SkipUntranslatedStrings { get; set; }

    [Display("Include pre-translated strings only")]
    public bool? IncludePreTranslatedStringsOnly { get; set; }

    [Display("Label IDs")]
    public IEnumerable<string>? LabelIds { get; set; }

    public DateTime? Deadline { get; set; }

    [Display("Date from")]
    public DateTime? DateFrom { get; set; }

    [Display("Date to")]
    public DateTime? DateTo { get; set; }
}