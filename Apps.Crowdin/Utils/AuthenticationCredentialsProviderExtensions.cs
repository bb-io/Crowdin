using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.Crowdin.Utils;

public static class AuthenticationCredentialsProviderExtensions
{
    public static string GetCrowdinPlan(this IEnumerable<AuthenticationCredentialsProvider> credentialsProviders)
    {
        var authenticationCredentialsProviders = credentialsProviders as AuthenticationCredentialsProvider[] ?? credentialsProviders.ToArray();
        
        if (authenticationCredentialsProviders.All(x => x.KeyName != CredsNames.CrowdinPlan))
        {
            throw new PluginMisconfigurationException("It seems like you haven't updated the connection and specified the plan you are using in Crowdin. " +
                                                      "Please update your connection or create a new one.");
        }
        
        return authenticationCredentialsProviders.Get(CredsNames.CrowdinPlan).Value;
    }
}