using Blackbird.Applications.Sdk.Common;

namespace Apps.Crowdin;

public class CrowdinApplication : IApplication
{
    public string Name
    {
        get => "Crowdin";
        set { }
    }

    public T GetInstance<T>()
    {
        throw new NotImplementedException();
    }
}