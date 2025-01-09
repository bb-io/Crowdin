using Apps.Crowdin.Connections;
using Apps.Crowdin.Constants;
using Blackbird.Applications.Sdk.Common.Authentication;
using FluentAssertions;
using Tests.Crowdin.Base;

namespace Tests.Crowdin;

[TestClass]
public class ValidatorTests : TestBase
{
    [TestMethod]
    public async Task ValidateConnection_ValidEnterpriseConnection_ShouldBeSuccessful()
    {
        var validator = new ConnectionValidator();

        var result = await validator.ValidateConnection(Creds, CancellationToken.None);
        result.IsValid.Should().Be(true);
        Console.WriteLine(result.Message);
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