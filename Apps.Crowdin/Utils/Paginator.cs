﻿using Blackbird.Applications.Sdk.Common.Exceptions;
using Crowdin.Api;

namespace Apps.Crowdin.Utils;

public static class Paginator
{
    private const int Limit = 50;
    
    public static async Task<List<T>> Paginate<T>(Func<int, int, Task<ResponseList<T>>> request)
    {
        try
        {
            var offset = 0;

            var items = new List<T>();
            ResponseList<T> response;
            do
            {
                response = await request(Limit, offset);
                offset += Limit;

                items.AddRange(response.Data ?? []);
            } while (response.Data?.Any() is true);

            return items;
        }
        catch (Exception e)
        {
            throw new PluginApplicationException(e.Message);
        }
    }
}