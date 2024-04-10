using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class TmFileFormatHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "Tmx", "TMX" },
        { "Csv", "CSV" },
        { "Xlsx", "XLSX" },
    };
}