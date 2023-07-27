﻿using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Microsoft.AspNetCore.WebUtilities;

namespace Apps.Crowdin.Connections.OAuth;

public class OAuth2AuthorizationSerivce : IOAuth2AuthorizeService
{
    public string GetAuthorizationUrl(Dictionary<string, string> values)
    {
        var parameters = new Dictionary<string, string>
        {
            { "scope", ApplicationConstants.Scope },
            { "client_id", ApplicationConstants.ClientId },
            { "redirect_uri", ApplicationConstants.RedirectUri },
            { "state", values["state"] },
            { "response_type", "code" },
        };
        
        return QueryHelpers.AddQueryString(Urls.OAuth, parameters);
    }
}