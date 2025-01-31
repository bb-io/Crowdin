﻿using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.RestSharp;
using HtmlAgilityPack;
using Newtonsoft.Json;
using RestSharp;

namespace Apps.Crowdin.Api.RestSharp.Basic;

public class CrowdinRestClient() : BlackBirdRestClient(new RestClientOptions { BaseUrl = new("https://api.crowdin.com/api/v2") })
{
    protected override Exception ConfigureErrorException(RestResponse response)
    {
        if (string.IsNullOrEmpty(response.Content))
        {
            if (string.IsNullOrEmpty(response.ErrorMessage))
            {
                throw new PluginApplicationException("Internal system error");
            }

            throw new PluginApplicationException(response.ErrorMessage);
        }

        if (response.ContentType?.Contains("application/json") == true || (response.Content.TrimStart().StartsWith("{") || response.Content.TrimStart().StartsWith("[")))
        {
            var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content!)!;
            throw new PluginApplicationException($"Code: {error.Error.Code}; Message: {error.Error.Message}");
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