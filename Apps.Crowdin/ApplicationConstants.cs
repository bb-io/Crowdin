namespace Apps.Crowdin;

public class ApplicationConstants
{
    public const string ClientId = "#{CROWDIN_CLIENT_ID}#";
    public const string ClientSecret = "#{CROWDIN_SECRET}#";
    public const string Scope =
        "tm mt group team user webhook project project.task project.status project.source project.webhook project.translation glossary";
}
