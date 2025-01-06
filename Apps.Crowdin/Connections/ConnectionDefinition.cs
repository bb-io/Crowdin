using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

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
                        new("basic", "(Basic) Crowdin"),
                        new("enterprise", "(Enterprise) Crowdin Enterprise")
                    ]
                }
            }
        },
    };

    public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(
        Dictionary<string, string> values)
    {
        return values.Select(x =>
            new AuthenticationCredentialsProvider(AuthenticationCredentialsRequestLocation.None, x.Key, x.Value));
    }
}