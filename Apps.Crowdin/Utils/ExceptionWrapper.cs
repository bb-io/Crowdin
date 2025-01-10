using Blackbird.Applications.Sdk.Common.Exceptions;
using Crowdin.Api;

namespace Apps.Crowdin.Utils;

public static class ExceptionWrapper
{
    public static async Task<T> ExecuteWithErrorHandling<T>(Func<Task<T>> func)
    {
        try
        {
            return await func.Invoke();
        }
        catch (CrowdinApiException e)
        {
            throw new PluginApplicationException(e.Message);
        }
    }
}