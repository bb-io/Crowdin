using Apps.Crowdin.Actions;
using Apps.Crowdin.Webhooks.Handlers.Organization;
using Apps.Crowdin.Webhooks.Lists;
using Blackbird.Applications.Sdk.Common.Authentication;

var actions = new ProjectActions();
var auth = new List<AuthenticationCredentialsProvider>
{
    new(AuthenticationCredentialsRequestLocation.Body, "apiToken",
        "1dc2075007cc715c2a39643a7d21ad562b3cd4ea3b3bad242af06e0410b69c66e7cea82b7b3dbd3b")
};

var list = new ProjectWebhookList();
var projectId = "603387";
var fileId = "1";
var storageId = "1913994713";
var stringId = "1";
var tmId = "405777";

var json = "{\n  \"event\": \"suggestion.added\",\n  \"project\": \"myproj2231\",\n  \"project_id\": \"603387\",\n  \"language\": \"es\",\n  \"source_string_id\": \"2814\",\n  \"translation_id\": \"1\",\n  \"user\": \"John Smith\",\n  \"user_id\": \"1\",\n  \"provider\": null,\n  \"is_pre_translated\": false,\n  \"file_id\": \"44\",\n  \"file\": \"/directory1/directory2/filename.extension\"\n}";
var w = await list.OnSuggestionAdded(new()
{
    Body = json
});

Console.ReadKey();