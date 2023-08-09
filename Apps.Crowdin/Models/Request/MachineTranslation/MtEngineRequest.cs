using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.MachineTranslation;

public class MtEngineRequest
{
    [Display("Machine translation engine")]
    [DataSource(typeof(MtEnginesDataHandler))]
    public string MtEngineId { get; set; }
}