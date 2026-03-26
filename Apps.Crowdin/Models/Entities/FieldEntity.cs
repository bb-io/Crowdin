using Blackbird.Applications.Sdk.Common;
using System.Collections;

namespace Apps.Crowdin.Models.Entities;

public class FieldEntity(string fieldKey, object fieldValue)
{
    [Display("Field key")]
    public string FieldKey { get; set; } = fieldKey;

    [Display("Field value")]
    public string FieldValue { get; set; } = ConvertValueToString(fieldValue);

    private static string ConvertValueToString(object value)
    {
        if (value == null) 
            return string.Empty;

        if (value is IEnumerable enumerable && value is not string)
        {
            var list = new List<string>();
            foreach (var item in enumerable)
                list.Add(item?.ToString() ?? string.Empty);

            return string.Join(", ", list);
        }

        if (value is bool b)
            return b ? "true" : "false";

        return value.ToString() ?? string.Empty;
    }
}
