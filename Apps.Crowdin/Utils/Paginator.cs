using Blackbird.Applications.Sdk.Common.Exceptions;
using Crowdin.Api;

namespace Apps.Crowdin.Utils;

public static class Paginator
{
    private const int MaxLimit = 500; // Crowdin API v2 maximum page size

    public static async Task<List<T>> Paginate<T>(Func<int, int, Task<ResponseList<T>>> request,
        int limit = MaxLimit)
    {
        try
        {
            var offset = 0;
            var items = new List<T>();

            while (true)
            {
                var page = (await request(limit, offset)).Data?.ToList() ?? [];
                items.AddRange(page);

                if (page.Count == 0)
                    return items;

                // Advancing by the page size instead of the requested limit keeps the paging correct
                // for endpoints that cap the page size below the requested one.
                offset += page.Count;
            }
        }
        catch (Exception e)
        {
            throw new PluginApplicationException(e.Message);
        }
    }
}