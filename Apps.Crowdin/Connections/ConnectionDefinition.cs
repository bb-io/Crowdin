using System.IdentityModel.Tokens.Jwt;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Utils.Extensions.Sdk;

namespace Apps.Crowdin.Connections;

public class ConnectionDefinition : IConnectionDefinition
{
    public IEnumerable<ConnectionPropertyGroup> ConnectionPropertyGroups => new List<ConnectionPropertyGroup>
    {
        new()
        {
            Name = "OAuth",
            AuthenticationType = ConnectionAuthenticationType.OAuth2,
            ConnectionProperties = new List<ConnectionProperty>()
        },
        new()
        {
            Name = "Other properties",
            AuthenticationType = ConnectionAuthenticationType.Undefined,
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
        },
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
    {
        var credentials = values
            .Select(x => new AuthenticationCredentialsProvider(x.Key, x.Value))
            .ToList();

        if (credentials.All(x => x.KeyName != CredsNames.CrowdinPlan))
        {
            throw new PluginMisconfigurationException("It seems like you haven't updated the connection and specified the plan you are using in Crowdin. " +
                                                      "Please update your connection or create a new one.");
        }

        var plan = credentials.GetCrowdinPlan();
        if (plan == Plans.Enterprise)
        {        
            var token = values.First(x => x.Key == CredsNames.ApiToken).Value;
            var domain = GetOrganization(token);
            credentials.Add(new(CredsNames.OrganizationDomain, domain));
        }

        return credentials;
    }

    private string GetOrganization(string token)
    {
        var jwt = new JwtSecurityToken(token);
        return jwt.Claims.FirstOrDefault(c => c.Type == CredsNames.OrganizationDomain)?.Value ??
               throw new("Wrong login to Crowdin Enterprise");
    }
}