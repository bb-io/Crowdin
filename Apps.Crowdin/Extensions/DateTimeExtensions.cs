namespace Apps.Crowdin.Extensions;

public static class DateTimeExtensions
{
    public static string ToIso8601Utc(this DateTime? dateTime)
    {
        return dateTime?.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")!;
    }
}
