using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.File
{
    public class AddNewSpreadsheetFileRequest : ManageFileRequest
    {
        [Display("File name")]
        public string? Name { get; set; }

        [Display("Branch ID")]
        public string? BranchId { get; set; }

        [Display("Directory ID")]
        public string? DirectoryId { get; set; }

        [Display("File context")]
        public string? Context { get; set; }

        public string? Title { get; set; }

        [Display("Excluded target languages")]
        public IEnumerable<string>? ExcludedTargetLanguages { get; set; }

        [Display("Attach label IDs")]
        public IEnumerable<int>? AttachLabelIds { get; set; }

        [Display("Content segmentation?")]
        [Description("If enabled, Crowdin will split content into segments")]
        public bool? ContentSegmentation { get; set; }

        [Display("First line contains header?")]
        [Description("If true, the first row in your spreadsheet file is treated as a header")]
        public bool? FirstLineContainsHeader { get; set; }

        [Display("Import translations?")]
        [Description("If true, existing translations will be imported")]
        public bool? ImportTranslations { get; set; }

        [Display("SRX storage ID")]
        [Description("Storage ID for the custom SRX segmentation file")]
        public long? SrxStorageId { get; set; }

        [Display("Context column number")]
        public int? ContextColumnNumber { get; set; }

        [Display("Identifier column number")]
        public int? IdentifierColumnNumber { get; set; }

        [Display("Labels column number")]
        public int? LabelsColumnNumber { get; set; }

        [Display("Max length column number")]
        public int? MaxLengthColumnNumber { get; set; }

        [Display("None column number")]
        public int? NoneColumnNumber { get; set; }

        [Display("Source or translation column number")]
        public int? SourceOrTranslationColumnNumber { get; set; }

        [Display("Source phrase column number")]
        public int? SourcePhraseColumnNumber { get; set; }

        [Display("Translation column number")]
        public int? TranslationColumnNumber { get; set; }

        [Display("Import each cell as a separate source string")]
        [Description("For XLSX files, if set to true, the request 'type' will be 'docx'")]
        public bool? ImportEachCellAsSeparateSourceString { get; set; }

    }
}
