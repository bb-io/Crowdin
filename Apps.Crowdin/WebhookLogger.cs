using System.Text;
using System.Text.Json;

namespace Apps.Crowdin;

public static class WebhookLogger
{
    private static readonly HttpClient Client = new();
    private const string Url = "https://webhook.site/b54a95ed-105f-45d6-9d98-4361b3fe5ee6";

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