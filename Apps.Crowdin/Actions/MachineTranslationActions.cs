using Apps.Crowdin.Api;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Response.MachineTranslation;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;

namespace Apps.Crowdin.Actions;

[ActionList]
public class MachineTranslationActions
{
    [Action("List machine translation engines", Description = "List all machine translation engines")]
    public async Task<ListMtEnginesResponse> ListMtEnginges(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        var client = new CrowdinClient(creds);

        var items = await Paginator.Paginate((lim, offset)
            => client.MachineTranslationEngines.ListMts(lim, offset));

        var mtEntities = items.Select(x => new MtEngineEntity(x)).ToArray();
        return new(mtEntities);
    }
    
    [Action("Translate via machine translation engine", Description = "Translate text via machine translation engine")]
    public async Task<ListMtEnginesResponse> TranslateTextViaMt(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        throw new NotImplementedException();
    }
    
    [Action("Translate lines via machine translation engine", Description = "Translate multiple text lines via machine translation engine")]
    public async Task<ListMtEnginesResponse> TranslateLinesViaMt(
        IEnumerable<AuthenticationCredentialsProvider> creds)
    {
        throw new NotImplementedException();
    }
}