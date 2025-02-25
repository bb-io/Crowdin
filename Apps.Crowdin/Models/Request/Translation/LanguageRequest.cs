using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Translation
{
    public class LanguageRequest
    {
        [Display("Language ID")]
        [DataSource(typeof(LanguagesDataHandler))]
        public string LanguageId { get; set; }
    }
}
