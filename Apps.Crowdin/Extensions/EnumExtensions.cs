using System.ComponentModel;

namespace Apps.Crowdin.Extensions;

public static class EnumExtensions
{
    public static string ToDescription(this Enum value)
    {
        var fi = value.GetType().GetField(value.ToString());
        if (fi == null) return value.ToString();

        var attr = fi.GetCustomAttributes(typeof(DescriptionAttribute), false)
            .OfType<DescriptionAttribute>()
            .FirstOrDefault();
        return attr?.Description ?? value.ToString();
    }
}