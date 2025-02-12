using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class TaskPatchPathHandler : IStaticDataSourceItemHandler
    {
        private static Dictionary<string, string> Data => new()
        {
            { "Status",         "/status" },
            { "Title",          "/title" },
            { "Description",    "/description" },
            { "Deadline",       "/deadline" },
            { "Started at",     "/startedAt" },
            { "Resolved at",    "/resolvedAt" },
            { "File IDs",       "/fileIds" },
            { "String IDs",     "/stringIds" },
            { "Assignees",      "/assignees" },
            { "Date from",      "/dateFrom" },
            { "Date to",        "/dateTo" },
            { "Label IDs",      "/labelIds" },
            { "Exclude label IDs", "/excludeLabelIds" }
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return Data.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
