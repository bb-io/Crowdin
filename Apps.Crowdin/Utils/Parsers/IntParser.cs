namespace Apps.Crowdin.Utils.Parsers;

public static class IntParser
{
    public static int? Parse(string? input, string errorName)
    {
        if (input is null)
            return null;
        
        if (!int.TryParse(input, out var intValue))
            throw new Exception("Group ID should be a number");

        return intValue;
    } 
}