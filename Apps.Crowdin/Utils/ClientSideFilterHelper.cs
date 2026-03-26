using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Filter;

namespace Apps.Crowdin.Utils;

public static class ClientSideFilterHelper
{
    public static IEnumerable<T> ApplyFieldsFilter<T>(
        this IEnumerable<T> items, 
        Func<T, IEnumerable<FieldEntity>> fieldsSelector,
        FieldsFilterRequest fieldsFilter)
    {
        if (fieldsFilter.FieldNamesFilter != null && fieldsFilter.FieldValuesFilter != null)
        {
            var filterPairs = fieldsFilter.FieldNamesFilter
                .Zip(fieldsFilter.FieldValuesFilter, (Name, Value) => new { Name, Value });

            items = items.Where(item =>
            {
                var fields = fieldsSelector(item);

                return filterPairs.All(filter =>
                    fields != null &&
                    fields.Any(f =>
                        f.FieldKey == filter.Name &&
                        f.FieldValue != null &&
                        f.FieldValue.Contains(filter.Value, StringComparison.OrdinalIgnoreCase)
                    )
                );
            });
        }

        return items;
    }
}
