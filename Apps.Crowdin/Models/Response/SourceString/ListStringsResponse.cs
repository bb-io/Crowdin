using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.SourceString;

public record ListStringsResponse(SourceStringEntity[] Strings);