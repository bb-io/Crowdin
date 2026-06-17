using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Files;

namespace Apps.Crowdin.Models.Response.Project;

public record ExportProjectAnalysisResponse(FileReference ExportedAnalysis)
{
    [Display("Analysis file")] 
    public FileReference ExportedAnalysis { get; set; } = ExportedAnalysis;
}