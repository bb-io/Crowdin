using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Authentication.OAuth2;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Apps.Crowdin.Connections.OAuth;

public class OAuth2TokenService(InvocationContext invocationContext)
    : BaseInvocable(invocationContext), IOAuth2TokenService, ITokenRefreshable
{
    private const string ExpiresAtKeyName = "expires_at";
    private const int RefreshBufferMinutes = 30;
    private const int MinimumRemainingLifetimeSeconds = 60;

    #region Token actions

    public Task<Dictionary<string, string>> RequestToken(string state, string code, Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "authorization_code" },
            { "client_id", ApplicationConstants.ClientId },
            { "client_secret", ApplicationConstants.ClientSecret },
            { "redirect_uri", $"{InvocationContext.UriInfo.BridgeServiceUrl.ToString().TrimEnd('/')}/AuthorizationCode" },
            { "state", state },
            { "code", code },
        };

        return GetToken(parameters, cancellationToken, "request token");
    }

    public Task<Dictionary<string, string>> RefreshToken(Dictionary<string, string> values,
        CancellationToken cancellationToken)
    {
        var refreshToken = values["refresh_token"];
        var parameters = new Dictionary<string, string>
        {
            { "grant_type", "refresh_token" },
            { "client_id", ApplicationConstants.ClientId },
            { "client_secret", ApplicationConstants.ClientSecret },
            { "refresh_token", refreshToken },
        };

        InvocationContext.Logger?.LogError(
            $"[Crowdin][OAuth] Starting refresh token flow. RefreshToken: {GetTokenPreview(refreshToken)}; LocalExpiresAt: {GetSafeValue(values, ExpiresAtKeyName)}; MinutesUntilLocalExpiry: {GetRefreshTokenExprireInMinutes(values)?.ToString() ?? "n/a"}",
            null);

        return GetToken(parameters, cancellationToken, "refresh token");
    }

    public Task RevokeToken(Dictionary<string, string> values)
    {
        throw new NotImplementedException();
    }

    #endregion

    #region Utils

    private async Task<Dictionary<string, string>> GetToken(
        Dictionary<string, string> parameters,
        CancellationToken token,
        string operationName)
    {
        try
        {
            var grantType = parameters.TryGetValue("grant_type", out var currentGrantType)
                ? currentGrantType
                : "unknown";
            var refreshTokenPreview = parameters.TryGetValue("refresh_token", out var currentRefreshToken)
                ? GetTokenPreview(currentRefreshToken)
                : "n/a";

            InvocationContext.Logger?.LogError(
                $"[Crowdin][OAuth] Requesting OAuth token. Operation: {operationName}; GrantType: {grantType}; RefreshToken: {refreshTokenPreview}",
                null);

            var responseContent = await ExecuteTokenRequest(parameters, token);

            InvocationContext.Logger?.LogError(
                $"[Crowdin][OAuth] OAuth token response body. Operation: {operationName}; GrantType: {grantType}; Body: {SanitizeOAuthResponseBody(responseContent)}",
                null);

            var resultDictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(responseContent)
                                       ?.ToDictionary(r => r.Key, r => r.Value?.ToString())
                                   ?? throw new InvalidOperationException($"Invalid response content: {responseContent}");

            var expiresIn = int.Parse(resultDictionary["expires_in"]);
            var utcNow = DateTime.UtcNow;
            var configuredBufferSeconds = RefreshBufferMinutes * 60;
            var effectiveBufferSeconds = expiresIn > MinimumRemainingLifetimeSeconds
                ? Math.Min(configuredBufferSeconds, expiresIn - MinimumRemainingLifetimeSeconds)
                : 0;
            var expiresAt = utcNow.AddSeconds(expiresIn - effectiveBufferSeconds);

            if (effectiveBufferSeconds != configuredBufferSeconds)
            {
                InvocationContext.Logger?.LogError(
                    $"[Crowdin][OAuth] OAuth token lifetime is shorter than the configured refresh buffer. Operation: {operationName}; GrantType: {grantType}; ExpiresInSeconds: {expiresIn}; RefreshBufferMinutes: {RefreshBufferMinutes}; EffectiveBufferSeconds: {effectiveBufferSeconds}",
                    null);
            }

            var nextRefreshToken = resultDictionary.TryGetValue("refresh_token", out var nextRefreshTokenValue) &&
                                   !string.IsNullOrWhiteSpace(nextRefreshTokenValue)
                ? nextRefreshTokenValue
                : currentRefreshToken ?? string.Empty;

            resultDictionary["refresh_token"] = nextRefreshToken;
            resultDictionary[ExpiresAtKeyName] = expiresAt.ToString("O");

            InvocationContext.Logger?.LogError(
                $"[Crowdin][OAuth] OAuth token request succeeded. Operation: {operationName}; GrantType: {grantType}; ExpiresInSeconds: {expiresIn}; RefreshBufferMinutes: {RefreshBufferMinutes}; EffectiveBufferSeconds: {effectiveBufferSeconds}; LocalExpiresAt: {expiresAt:O}; NextRefreshToken: {GetTokenPreview(nextRefreshToken)}; RefreshTokenReturned: {!string.IsNullOrWhiteSpace(nextRefreshTokenValue)}",
                null);

            return resultDictionary;
        }
        catch (Exception ex) when (ex is not PluginApplicationException && ex is not InvalidOperationException)
        {
            InvocationContext.Logger?.LogError(
                $"[Crowdin][OAuth] Unexpected error during {operationName}. Error: {ex.Message}",
                null);

            throw new PluginApplicationException($"Unexpected error during {operationName}: {ex.Message}", ex);
        }
    }

    private async Task<string> ExecuteTokenRequest(Dictionary<string, string> parameters,
        CancellationToken cancellationToken)
    {
        using var client = new HttpClient();
        using var content = new FormUrlEncodedContent(parameters);
        using var response = await client.PostAsync(Urls.Token, content, cancellationToken);
        var responseContent = await response.Content.ReadAsStringAsync(cancellationToken);

        if (!response.IsSuccessStatusCode || string.IsNullOrWhiteSpace(responseContent))
        {
            var grantType = parameters.TryGetValue("grant_type", out var currentGrantType)
                ? currentGrantType
                : "unknown";

            InvocationContext.Logger?.LogError(
                $"[Crowdin][OAuth] OAuth token request failed. GrantType: {grantType}; StatusCode: {(int)response.StatusCode} {response.StatusCode}",
                null);
            InvocationContext.Logger?.LogError(
                $"[Crowdin][OAuth] OAuth token response body. GrantType: {grantType}; Body: {SanitizeOAuthResponseBody(responseContent)}",
                null);

            throw new PluginApplicationException(
                $"Failed to obtain OAuth token: {response.StatusCode}. {responseContent}");
        }

        return responseContent;
    }

    public bool IsRefreshToken(Dictionary<string, string> values)
    {
        if (!values.TryGetValue(ExpiresAtKeyName, out var expireValue))
            return false;

        return DateTime.TryParse(expireValue, out var expiresAt) && DateTime.UtcNow > expiresAt;
    }

    public int? GetRefreshTokenExprireInMinutes(Dictionary<string, string> values)
    {
        if (!values.TryGetValue(ExpiresAtKeyName, out var expireValue))
            return null;

        if (!DateTime.TryParse(expireValue, out var expireDate))
            return null;

        var difference = expireDate - DateTime.UtcNow;

        return (int)difference.TotalMinutes;
    }

    private static string GetSafeValue(Dictionary<string, string> values, string key)
        => values.TryGetValue(key, out var value) ? value : "n/a";

    private static string GetTokenPreview(string? token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return "missing";

        return token.Length <= 8 ? token : token[..8];
    }

    private static string SanitizeOAuthResponseBody(string? content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return "empty";

        try
        {
            var json = JToken.Parse(content);

            if (json is JObject obj)
            {
                MaskToken(obj, "access_token");
                MaskToken(obj, "refresh_token");
                MaskToken(obj, "id_token");
            }

            return json.ToString(Formatting.None);
        }
        catch
        {
            return content;
        }
    }

    private static void MaskToken(JObject obj, string key)
    {
        if (obj.TryGetValue(key, out var tokenValue) && tokenValue.Type == JTokenType.String)
            obj[key] = GetTokenPreview(tokenValue.Value<string>());
    }

    #endregion
}
