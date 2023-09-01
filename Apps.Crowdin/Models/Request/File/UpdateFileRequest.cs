using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Crowdin.Models.Request.File
{
    public class UpdateFileRequest
    {
        [Display("File ID")]
        public string FileId { get; set; }

        public Blackbird.Applications.Sdk.Common.Files.File? File { get; set; }

        [Display("Update option")]
        [DataSource(typeof(FileUpdateOptionHandler))]
        public string? UpdateOption { get; set; }

    }
}
