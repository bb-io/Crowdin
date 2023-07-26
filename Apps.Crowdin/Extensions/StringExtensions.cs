using System.Text.RegularExpressions;

namespace Apps.Crowdin.Extensions;

public static class StringExtensions
{
    public static string ToPascalCase(this string input)
        => Regex.Replace(input, @"\b\p{Ll}", match => match.Value.ToUpper());
}