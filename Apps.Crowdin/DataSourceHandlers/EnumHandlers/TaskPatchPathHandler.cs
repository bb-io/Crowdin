using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class TaskPatchPathHandler : IStaticDataSourceItemHandler
    {
        private static Dictionary<string, string> Data => new()
        {
            {"/status" , "Status"},
            {"/title" ,"Title"},
            {"/description","Description"},
            {"/deadline" , "Deadline"},
            {"/startedAt" ,"Started at"},
            {"/resolvedAt" ,"Resolved at"},
            {"/fileIds","File IDs"},
            {"/stringIds" ,"String IDs"},
            {"/assignees", "Assignees"},
            {"/dateFrom" , "Date from"},
            {"/dateTo" ,"Date to"},
            {"/labelIds" ,"Label IDs"},
            {"/excludeLabelIds" , "Exclude label IDs"}
        };

        public IEnumerable<DataSourceItem> GetData()
        {
            return Data.Select(x => new DataSourceItem(x.Key, x.Value));
        }
    }
}
