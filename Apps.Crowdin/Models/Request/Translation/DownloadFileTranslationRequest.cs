using Blackbird.Applications.Sdk.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Crowdin.Models.Request.Translation
{
    public class DownloadFileTranslationRequest
    {
        [Display("File ID")]
        public string FileId { get; set; }

        [Display("Target language")]
        public string TargetLanguage { get; set; }

        [Display("Skip untranslated strings?")]
        public bool? SkipUntranslatedStrings { get; set; }

        [Display("Skip untranslated files?")]
        public bool? SkipUntranslatedFiles { get; set; }

        [Display("Export approved only?")]
        public bool? ExportApprovedOnly { get; set; }
    }
}
