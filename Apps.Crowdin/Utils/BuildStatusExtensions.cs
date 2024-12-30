using Crowdin.Api;

namespace Apps.Crowdin.Utils;

public static class BuildStatusExtensions
{
    public static string ToReadableString(this BuildStatus buildStatus)
    {
        switch (buildStatus)
        {
            case BuildStatus.Created:
                return "created";
            case BuildStatus.InProgress:
                return "in_progress";
            case BuildStatus.Canceled:
                return "canceled";
            case BuildStatus.Failed:
                return "failed";
            case BuildStatus.Finished:
                return "finished";
            default:
                throw new ArgumentException("Unsupported build type");
        }
    }
}