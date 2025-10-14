using Apps.Crowdin.Models.Dtos;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Newtonsoft.Json;
using RestSharp;
using System.Net;

namespace Apps.Crowdin.Utils;

public static class ErrorHelper
{
    public static Exception ProcessJsonError(RestResponse response)
    {
        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var error = JsonConvert.DeserializeObject<ErrorResponse>(response.Content!)!;
            var firstError = error.Errors.FirstOrDefault()?.Error;
            var firstDetail = firstError?.Errors.FirstOrDefault();

            throw new PluginApplicationException(
                $"Key: {firstError?.Key}; Code: {firstDetail?.Code}; Message: {firstDetail?.Message}"
            );
        }
        else
        {
            var error = JsonConvert.DeserializeObject<ErrorDto>(response.Content!)!;
            throw new PluginApplicationException($"Code: {error.Error.Code}; Message: {error.Error.Message}");
        }
    }
}
