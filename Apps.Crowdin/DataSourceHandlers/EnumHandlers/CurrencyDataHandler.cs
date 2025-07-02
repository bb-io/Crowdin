using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.DataSourceHandlers.EnumHandlers
{
    public class CurrencyDataHandler : IStaticDataSourceItemHandler
    {
        private static readonly Dictionary<string, string> Data = new()
        {
            { "USD", "USD" },
            { "EUR", "EUR" },
            { "JPY", "JPY" },
            { "GBP", "GBP" },
            { "AUD", "AUD" },
            { "CAD", "CAD" },
            { "CHF", "CHF" },
            { "CNY", "CNY" },
            { "SEK", "SEK" },
            { "NZD", "NZD" },
            { "MXN", "MXN" },
            { "SGD", "SGD" },
            { "HKD", "HKD" },
            { "NOK", "NOK" },
            { "KRW", "KRW" },
            { "TRY", "TRY" },
            { "RUB", "RUB" },
            { "INR", "INR" },
            { "BRL", "BRL" },
            { "ZAR", "ZAR" },
            { "GEL", "GEL" },
            { "UAH", "UAH" },
            { "DDK", "DDK" }
        };

        public IEnumerable<DataSourceItem> GetData()
            => Data.Select(x => new DataSourceItem(x.Key, x.Value));
    }
}
