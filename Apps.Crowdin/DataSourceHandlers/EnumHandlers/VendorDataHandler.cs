using Blackbird.Applications.Sdk.Common.Dictionaries;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers;

public class VendorDataHandler : IStaticDataSourceHandler
{
    public Dictionary<string, string> GetData() => new()
    {
        { "crowdin_language_service", "Crowdin Language Services" },
        { "oht", "OneHourTranslation" },
        { "gengo", "Gengo" },
        { "translated", "Translated.com" },
        { "alconost", "Alconost" },
        { "babbleon", "Babble-on" },
        { "tomedes", "Tomedes" },
        { "e2f", "e2f" },
        { "write_path_admin", "WritePath" },
        { "inlingo", "Inlingo" },
        { "acclaro", "Acclaro" },
        { "translate_by_humans", "Translate by Humans" },
        { "lingo24", "Lingo24" },
        { "assertio_language_services", "ASSERTIO Language Services" },
        { "gte_localize", "GTE Localize" },
        { "kettu_solutions", "Kettu Solutions" },
        { "languageline_solutions", "LanguageLine Translation Solutions" },
    };
}