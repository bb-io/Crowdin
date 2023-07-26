using Newtonsoft.Json;

namespace Apps.Crowdin;

public static class Logger
{
    public static Task Log(object payload)
    {
        var client = new HttpClient();
        var request = new HttpRequestMessage(HttpMethod.Post, "https://webhook.site/37aecaf2-ce6b-4817-8788-79eb3a06832f");
        request.Content = new StringContent(JsonConvert.SerializeObject(payload));

        return client.SendAsync(request);
    }
}