using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Parsers;

namespace Apps.Crowdin.Utils
{
    public class ParsingUtils
    {
        public static T ParseOrThrow<T>(string input, string fieldName, Func<string, T?> parseFunc) where T : struct
        {
            try
            {
                var result = parseFunc(input);
                if (!result.HasValue)
                {
                    throw new PluginMisconfigurationException($"The value for '{fieldName}' is invalid: {input}");
                }
                return result.Value;
            }
            catch (Exception ex)
            {
                throw new PluginMisconfigurationException($"Failed to parse '{fieldName}'. Error: {ex.Message}", ex);
            }
        }
    }
}
