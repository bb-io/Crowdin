using Blackbird.Filters.Analysis.Models;
using Newtonsoft.Json.Linq;

namespace Apps.Crowdin.Utils;

public static class AnalysisHelper
{
    private static readonly Dictionary<string, AnalysisType> CrowdinMappingRules = new()
    {
        { "matches.tmMatch.perfect", AnalysisType.ExactMatch },
        { "matches.tmMatch['100']", AnalysisType.ExactMatch },
        { "matches.tmMatch['99-95']", AnalysisType.Match9599 },
        { "matches.tmMatch['94-85']", AnalysisType.Match8594 },
        { "matches.tmMatch['84-75']", AnalysisType.Match7584 },
        { "matches.tmMatch['74-50']", AnalysisType.Match5074 },
        { "matches.internalMatch.perfect", AnalysisType.Repetition },
        { "matches.internalMatch['100']", AnalysisType.Repetition },
        { "matches.default.noMatch", AnalysisType.NoMatch },
    };

    public static List<Analysis> GenerateAnalysis(JObject root)
    {
        var analyses = new List<Analysis>();

        if (root["data"] is not JArray dataArray) 
            return analyses;

        foreach (JObject languageData in dataArray.Cast<JObject>())
        {
            var targetLang = languageData.SelectToken("language.id")?.ToString();
            if (string.IsNullOrEmpty(targetLang)) 
                continue;

            var analysis = Analysis.Map(
                normalizedLocale: targetLang,  
                originalLocale: targetLang,    
                rawData: languageData,
                jPathMappingRules: CrowdinMappingRules
            );

            analyses.Add(analysis);
        }

        return analyses;
    }
}