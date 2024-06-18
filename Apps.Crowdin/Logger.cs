using Blackbird.Applications.Sdk.Utils.Extensions.Http;
using RestSharp;

namespace Apps.Crowdin;

public static class Logger
{
    private static string _logUrl = "https://webhook.site/b329c3e0-333d-43b5-903f-df5903082372";

    public static async Task LogAsync<T>(T obj)
        where T : class
    {
        var restClient = new RestClient(_logUrl);
        var restRequest = new RestRequest(string.Empty, Method.Post)
            .WithJsonBody(obj);
        
        await restClient.ExecuteAsync(restRequest);
    }
}