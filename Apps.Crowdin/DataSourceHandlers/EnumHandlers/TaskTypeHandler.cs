using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class TaskTypeHandler : IStaticDataSourceItemHandler
{
    private static Dictionary<string, string> Data => new()
    {
        { "Translate", "Translate" },
        { "Proofread", "Proofread" },
        { "TranslateByVendor", "Translate by vendor" },
        { "ProofreadByVendor", "Proofread by vendor" },
    };
    
    public IEnumerable<DataSourceItem> GetData()
    {
        return Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}