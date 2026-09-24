using Blackbird.Applications.Sdk.Utils.Parsers;

namespace Apps.Crowdin.Extensions;

public static class StringExtensions
{
    public static int? ToPlanScopedInt(this string? value, string currentPlan, string supportedPlan, string paramDisplayName)
    {
        if (string.IsNullOrEmpty(value) || currentPlan != supportedPlan)
            return null;

        return IntParser.Parse(value, paramDisplayName);
    }
}