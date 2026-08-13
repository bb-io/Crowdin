using System.IdentityModel.Tokens.Jwt;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Common.Exceptions;

namespace Apps.Crowdin.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = ConnectionNames.OAuth,
            DisplayName= "OAuth",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties = new List<ConnectionProperty>
            {
             new(CredsNames.CrowdinPlan)
                {
                    DisplayName = "Crowdin plan",
                    Description = "The plan of the Crowdin account. It could be either Basic or Enterprise. " +
                                  "See more at https://crowdin.com/pricing",
                    DataItems =
                    [
                        new(Plans.Basic, "(Basic) Crowdin"),
                        new(Plans.Enterprise, "(Enterprise) Crowdin Enterprise")
                    ]
             }
            }
        }
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
    {
        var credentials = values
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value))
            .ToList();

        if (credentials.GetCrowdinPlan() != Plans.Enterprise)
            return credentials;

        if (!values.TryGetValue(CredsNames.ApiToken, out var token) || string.IsNullOrWhiteSpace(token))
        {
            throw new PluginMisconfigurationException(
                "The Crowdin Enterprise connection has no access token stored, which usually means the " +
                "authorization was never completed. Please reconnect your Crowdin connection.");
        }

        credentials.Add(new(CredsNames.OrganizationDomain, GetOrganization(token)));

        return credentials;
    }

    private static string GetOrganization(string token)
    {
        var handler = new JwtSecurityTokenHandler();

        if (!handler.CanReadToken(token))
        {
            throw new PluginMisconfigurationException(
                "The stored Crowdin Enterprise access token could not be read. " +
                "Please reconnect your Crowdin connection.");
        }

        JwtSecurityToken jwt;
        try
        {
            jwt = handler.ReadJwtToken(token);
        }
        catch (Exception e)
        {
            throw new PluginMisconfigurationException(
                $"The stored Crowdin Enterprise access token could not be read: {e.Message}. " +
                "Please reconnect your Crowdin connection.");
        }

        return jwt.Claims.FirstOrDefault(c => c.Type == CredsNames.OrganizationDomain)?.Value ??
               throw new PluginMisconfigurationException(
                   "Could not read the organization domain from the Crowdin Enterprise access token. " +
                   "Please make sure you signed in with a Crowdin Enterprise account and reconnect " +
                   "your Crowdin connection.");
    }
}