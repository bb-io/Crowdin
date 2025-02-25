using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin.Models.Request.File
{
    public class FileIdsRequest
    {
        [Display("File ID`s")]
        public IEnumerable<string> Ids { get; set; }
    }
}
