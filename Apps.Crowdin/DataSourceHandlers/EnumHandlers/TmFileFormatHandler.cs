using Blackbird.Applications.Sdk.Utils.Sdk.DataSourceHandlers;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class TmFileFormatHandler : EnumDataHandler
{
    protected override Dictionary<string, string> EnumValues => new()
    {
        { "Tmx", "TMX" },
        { "Csv", "CSV" },
        { "Xlsx", "XLSX" },
    };
}