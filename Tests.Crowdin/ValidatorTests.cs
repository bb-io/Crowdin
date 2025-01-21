using Apps.Crowdin.Api.RestSharp.Basic;
using Apps.Crowdin.Connections;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Exceptions;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using RestSharp;
using Tests.Crowdin.Base;

namespace Tests.Crowdin;

[TestClass]
public class ValidatorTests : TestBase
{
    private TestCrowdinRestClient _client;

    [TestInitialize]
    public void Setup()
    {
        _client = new TestCrowdinRestClient();
    }


    [TestMethod]
    public void ConfigureErrorException_EmptyContentAndEmptyErrorMessage_ShouldReturnException()
    {
        var response = new RestResponse
        {
            Content = "",
            ErrorMessage = ""
        };
        Action act = () => _client.PublicConfigureErrorException(response);

        act.Should().ThrowExactly<Exception>();
    }

    [TestMethod]
    public void ConfigureErrorException_EmptyContentWithErrorMessage_ShouldReturnPluginApplicationException()
    {
        var response = new RestResponse
        {
            Content = "",
            ErrorMessage = "Some error occurred."
        };

        Action act = () => _client.PublicConfigureErrorException(response);

        act.Should().ThrowExactly<PluginApplicationException>();
    }

    [TestMethod]
    public void ConfigureErrorException_ValidHtmlErrorResponse_ShouldReturnPluginApplicationException()
    {
        var htmlContent = "<html><head><title>Error</title></head><body><p>Something went wrong.</p></body></html>";
        var response = new RestResponse
        {
            Content = htmlContent,
            ContentType = "text/html"
        };
        Action act = () => _client.PublicConfigureErrorException(response);
        act.Should().ThrowExactly<PluginApplicationException>();
    }

    [TestMethod]
    public void ConfigureErrorException_ValidJsonErrorResponse_ShouldThrowPluginApplicationException()
    {
        var jsonContent = "{\"error\": {\"code\": \"123\", \"message\": \"Invalid configuration.\"}}";
        var response = new RestResponse
        {
            Content = jsonContent,
            ContentType = "application/json"
        };
        Action act = () => _client.PublicConfigureErrorException(response);

        act.Should().ThrowExactly<PluginApplicationException>();
    }

    [TestMethod]
    public void ConfigureErrorException_JsonErrorResponse_ShouldThrowPluginApplicationException()
    {
        var jsonContent = "{\"error\": {\"code\": \"123\", \"message\": \"Invalid configuration.\"}}";
        var response = new RestResponse
        {
            Content = jsonContent,
            ContentType = ""
        };
        Action act = () => _client.PublicConfigureErrorException(response);

        act.Should().ThrowExactly<PluginApplicationException>();
    }

    [TestMethod]
    public void ConfigureErrorException_UnknownError_ShouldThrowPluginApplicationException()
    {
        var unknownContent = "This is an unknown response format.";
        var response = new RestResponse
        {
            Content = unknownContent,
            ContentType = "application/unknown"
        };

        Action act = () => _client.PublicConfigureErrorException(response);
        act.Should().Throw<PluginApplicationException>();          
    }




    [TestMethod]
    public async Task ValidateConnection_ValidEnterpriseConnection_ShouldBeSuccessful()
    {
        var validator = new ConnectionValidator();

        var result = await validator.ValidateConnection(Creds, CancellationToken.None);
        Console.WriteLine(result.Message);
        result.IsValid.Should().Be(true);
    }

    [TestMethod]
    public async Task ValidateConnection_InvalidConnection_ShouldFail()
    {
        var validator = new ConnectionValidator();

        var newCredentials = Creds.Select(x => new AuthenticationCredentialsProvider(x.KeyName, x.Value + "_incorrect"));
        var result = await validator.ValidateConnection(newCredentials, CancellationToken.None);
        result.IsValid.Should().Be(false);
    }
}