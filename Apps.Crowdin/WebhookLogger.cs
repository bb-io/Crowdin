using System.Text;
using System.Text.Json;

namespace Apps.Crowdin;

public static class WebhookLogger
{
    private static readonly HttpClient Client = new();
    private const string Url = "https://webhook.site/7dab2431-84d4-4bbe-80a8-91809dab9912";

    public static void Log(object body)
    {
        try
        {
            var json = JsonSerializer.Serialize(body);
            using var request = new HttpRequestMessage(HttpMethod.Post, Url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = Client.Send(request);
        }
        catch (Exception ex)
        {
            var json = JsonSerializer.Serialize(ex.Message);
            using var request = new HttpRequestMessage(HttpMethod.Post, Url);
            request.Content = new StringContent(json, Encoding.UTF8, "application/json");

            using var response = Client.Send(request);
        }
    }
}