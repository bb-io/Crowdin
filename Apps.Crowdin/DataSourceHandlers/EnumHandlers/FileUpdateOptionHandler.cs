using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;
using Crowdin.Api.SourceFiles;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class FileUpdateOptionHandler : EnumDataHandler
    {
        protected override Dictionary<string, string> EnumValues => new()
        {
            { "clear_translations_and_approvals", "Clear translations and approvals" },
            { "keep_translations", "Keep translations" },
            { "keep_translations_and_approvals", "Keep translations and approvals" },
        };
    }
}