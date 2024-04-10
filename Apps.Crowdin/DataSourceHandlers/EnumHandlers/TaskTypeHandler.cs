using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class TaskTypeHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "Translate", "Translate" },
        { "Proofread", "Proofread" },
        { "TranslateByVendor", "Translate by vendor" },
        { "ProofreadByVendor", "Proofread by vendor" },
    };
}