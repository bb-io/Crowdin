using Apps.Crowdin.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;

var actions = new TranslationActions();
var auth = new List<AuthenticationCredentialsProvider>
{
    new(AuthenticationCredentialsRequestLocation.Body, "apiToken",
        "1dc2075007cc715c2a39643a7d21ad562b3cd4ea3b3bad242af06e0410b69c66e7cea82b7b3dbd3b")
};

var projectId = "603387";
var fileId = "1";
var storageId = "1913994713";
var stringId = "1";
var tmId = "405777";


var tr = await actions.ListLangTranslations(auth, new()
{
    ProjectId  = projectId,
    LanguageId = "uk"
});

Console.ReadKey();