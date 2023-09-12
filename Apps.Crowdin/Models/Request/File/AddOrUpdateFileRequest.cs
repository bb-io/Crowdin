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
    public class AddOrUpdateFileRequest : AddNewFileRequest
    {
        [Display("Update option")]
        [DataSource(typeof(FileUpdateOptionHandler))]
        public string? UpdateOption { get; set; }
    }
}
