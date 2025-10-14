namespace Apps.Crowdin.Extensions;

public static class DictionaryExtensions
{
    public static void AddIfNotNullOrEmpty(this IDictionary<string, object> dict, string key, string? value)
    {
        if (!string.IsNullOrEmpty(value))
            dict[key] = value;
    }

    public static void AddIfHasValue(this IDictionary<string, object> dict, string key, DateTime? value)
    {
        if (value.HasValue)
            dict[key] = value.ToIso8601Utc();
    }
}
