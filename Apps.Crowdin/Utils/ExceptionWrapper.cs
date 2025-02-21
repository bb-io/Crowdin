using Blackbird.Applications.Sdk.Common.Exceptions;
using Crowdin.Api;

namespace Apps.Crowdin.Utils;

public static class ExceptionWrapper
{
    private static readonly List<string> MisconfigurationErrorMessages =
    [
        "not found or does not exist",
        "File name can't contain any of the following characters"
    ];

    public static async Task<T> ExecuteWithErrorHandling<T>(Func<Task<T>> func)
    {
        try
        {
            return await func.Invoke();
        }
        catch (CrowdinApiException e)
        {
            throw new PluginApplicationException($"Crowdin answer: {e.Message}");
        }
        catch (Exception e)
        {
            throw new PluginApplicationException(e.Message);
        }
    }
    
    public static async Task ExecuteWithErrorHandling(Func<Task> func)
    {
        try
        {
            await func.Invoke();
        }
        catch (CrowdinApiException e)
        {
            throw new PluginApplicationException($"Crowdin answer: {e.Message}");
        }
        catch (Exception e)
        {
            throw new PluginApplicationException(e.Message);
        }
    }
}