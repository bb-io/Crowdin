using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class WorkflowStepTypeHandler : IStaticDataSourceItemHandler
    {
        private static Dictionary<string, string> Data => new()
        {
            { "Translate", "Translation" },
            { "Proofreading", "Proofreading" },
            { "TranslateByVendor", "Translation by Vendor" },
            { "ProofreadByVendor", "Proofreading by Vendor" },
            { "TMPreTranslate", "TM Pre-translation" },
            { "MachinePreTranslate", "MT Pre-translation" },
            { "Crowdsource", "Crowdsourcing" }
        };

        public IEnumerable<DataSourceItem> GetData()
            => Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}
