using Apps.Crowdin.Factories;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Connections;

namespace Apps.Crowdin.Connections;

public class ConnectionValidator : IConnectionValidator
{
    public async ValueTask<ConnectionValidationResponse> ValidateConnection(
        IEnumerable<AuthenticationCredentialsProvider> authProviders, CancellationToken cancellationToken)
    {
        IApiClientFactory factory = new ApiClientFactory();
        
        var authenticationCredentialsProviders = authProviders as AuthenticationCredentialsProvider[] ?? authProviders.ToArray();
        var client = factory.BuildSdkClient(authenticationCredentialsProviders);
        
        try
        {
            await client.Languages.ListSupportedLanguages();

            return new()
            {
                IsValid = true
            };
        }
        catch (Exception ex)
        {
            return new()
            {
                IsValid = false,
                Message = ex.Message
            };
        }
    }
}