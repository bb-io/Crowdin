﻿using Apps.Crowdin.Api;
using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Api.RestSharp.Enterprise;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;
using Crowdin.Api;
using RestSharp;

namespace Apps.Crowdin.Factories;

public class ApiClientFactory : IApiClientFactory
{
    public string GetPlan(IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        return credentialsProviders.Get(CredsNames.CrowdinPlan).Value;
    }

    public RestClient BuildRestClient(IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        var authenticationCredentialsProviders = credentialsProviders as AuthenticationCredentialsProvider[] ?? credentialsProviders.ToArray();
        var crowdinPlan = authenticationCredentialsProviders.GetCrowdinPlan();
        
        if (crowdinPlan == Plans.Basic)
        {
            return new CrowdinRestClient();
        }

        if (crowdinPlan == Plans.Enterprise)
        {
            return new CrowdinEnterpriseRestClient(authenticationCredentialsProviders);
        }

        throw new Exception($"Unsupported crowdin plan provided: {crowdinPlan}");
    }

    public CrowdinClient BuildSdkClient(IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        var authenticationCredentialsProviders = credentialsProviders as AuthenticationCredentialsProvider[] ?? credentialsProviders.ToArray();
        var crowdinPlan = authenticationCredentialsProviders.GetCrowdinPlan();

        var credentials = new CrowdinCredentials
        {
            AccessToken = authenticationCredentialsProviders.Get(CredsNames.ApiToken).Value
        };

        if (crowdinPlan == Plans.Enterprise)
        {
            credentials.Organization = authenticationCredentialsProviders.Get(CredsNames.OrganizationDomain).Value;
        }
        
        return new CrowdinClient(credentials, authenticationCredentialsProviders);
    }
}