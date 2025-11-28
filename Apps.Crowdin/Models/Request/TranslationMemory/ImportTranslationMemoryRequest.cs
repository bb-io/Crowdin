using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Crowdin.Models.Request.TranslationMemory
{
    public class ImportTranslationMemoryRequest
    {
        [Display("Translation memory file")]
        public FileReference File { get; set; } = default!;
    }
}
