using Apps.Crowdin.Extensions;

namespace Apps.Crowdin.Utils.Parsers;

public static class EnumParser
{
    public static T? Parse<T>(string? input, string variableName, string[] acceptableValues) where T : struct
    {
        if (input is null)
            return null;

        if (!Enum.TryParse(input, ignoreCase: true, out T res))
            throw new($"Wrong {variableName.ToPascalCase()} value, acceptable values are: {string.Join(", ", acceptableValues)}");

        return res;
    }
}