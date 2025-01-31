using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Parsers;

namespace Apps.Crowdin.Utils
{
    public static class ParsingUtils
    {
        public static int ParseOrThrow(string input, string fieldName)
        {
            try
            {
                return IntParser.Parse(input, fieldName)
                       ?? throw new PluginMisconfigurationException($"The value of {fieldName} is invalid: {input}");
            }
            catch (Exception ex)
            {
                throw new PluginMisconfigurationException($"Failed to parse {fieldName}. Error: {ex.Message}", ex);
            }
        }
    }
}
