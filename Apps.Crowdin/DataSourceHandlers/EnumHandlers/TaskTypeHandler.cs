using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class TaskTypeHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "Translate", "Translate" },
        { "Proofread", "Proofread" },
        { "TranslateByVendor", "Translate by vendor" },
        { "ProofreadByVendor", "Proofread by vendor" },
    };
}