using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.Models.Request.File
{
    public class FileIdsRequest
    {
        [Display("File ID`s")]
        public IEnumerable<string> Ids { get; set; }

        [Display("Format")]
        [StaticDataSource(typeof(ExportFileFormatHandler))]
        public string Format { get; set; }
    }
}
