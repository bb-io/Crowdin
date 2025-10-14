using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Api.RestSharp.Basic;

public class CrowdinRestClient() : BlackBirdRestClient(new RestClientOptions { BaseUrl = new("https://api.crowdin.com/api/v2"), Timeout= TimeSpan.FromSeconds(180)})
{
    private const int MaxRetries = 3;
    private const int DelayBetweenRetriesMs = 2000;

    public override async Task<T> ExecuteWithErrorHandling<T>(RestRequest request)
    {
        string content = (await ExecuteWithErrorHandling(request)).Content;
        T val = JsonConvert.DeserializeObject<T>(content, JsonSettings);
        if (val == null)
        {
            throw new Exception($"Could not parse {content} to {typeof(T)}");
        }

        return val;
    }

    public override async Task<RestResponse> ExecuteWithErrorHandling(RestRequest request)
    {
        int retryCount = 0;

        while (true)
        {
            RestResponse response = await ExecuteAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                if (!string.IsNullOrEmpty(response.Content) &&
                    response.Content.Contains("Other files are currently being processed") &&
                    retryCount < MaxRetries)
                {
                    retryCount++;
                    await Task.Delay(DelayBetweenRetriesMs);
                    continue;
                }

                throw ConfigureErrorException(response);
            }

            return response;
        }
    }

    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (string.IsNullOrEmpty(response.Content))
        {
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new Exception("Internal system error");
            }

            throw new PluginApplicationException(response.ErrorMessage);
        }

        if (response.Content.Contains("New-line characters are not allowed in header values"))
        {
            throw new PluginApplicationException(
                "New-line characters are not allowed in header values. " +
                "Please remove any line breaks from the file name and try again."
            );
        }

        if (response.ContentType?.Contains("application/json") == true || (response.Content.TrimStart().StartsWith("{") || response.Content.TrimStart().StartsWith("[")))
        {
            var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content!)!;
            var firstError = error.Errors.FirstOrDefault()?.Error;
            var firstDetail = firstError?.Errors.FirstOrDefault();

            throw new PluginApplicationException(
                $"Key: {firstError?.Key}; Code: {firstDetail?.Code}; Message: {firstDetail?.Message}"
            );
        }
        else if (response.ContentType?.Contains("text/html", StringComparison.OrdinalIgnoreCase) == true || response.Content.StartsWith("<"))
        {
            var errorMessage = ExtractHtmlErrorMessage(response.Content);
            throw new PluginApplicationException(errorMessage);
        }
        else
        {
            var errorMessage = $"Error: {response.ContentType}. Response Content: {response.Content}";
            throw new PluginApplicationException(errorMessage);
        }
    }

    private string ExtractHtmlErrorMessage(string html)
    {
        if (string.IsNullOrEmpty(html)) return "N/A";

        var htmlDoc = new HtmlDocument();
        htmlDoc.LoadHtml(html);

        var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//title");
        var bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

        var title = titleNode?.InnerText.Trim() ?? "No Title";
        var body = bodyNode?.InnerText.Trim() ?? "No Description";
        return $"{title}: \nError Description: {body}";
    }
}