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
                ConnectionUsage = ConnectionUsage.Actions,
                ConnectionProperties = new List<ConnectionProperty>()
            }
        };

        public IEnumerable<AuthenticationCredentialsProvider> CreateAuthorizationCredentialsProviders(Dictionary<string, string> values)
        {
            var accessToken = values.First(v => v.Key == "access_token");
            
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                CredsNames.ApiToken,
                accessToken.Value
            );
            
            var refreshToken = values.First(v => v.Key == "refresh_token");
            yield return new AuthenticationCredentialsProvider(
                AuthenticationCredentialsRequestLocation.None,
                CredsNames.RefreshToken,
                refreshToken.Value
            );
        }
}