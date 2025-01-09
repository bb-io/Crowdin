using Apps.Crowdin.Api;
using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.MachineTranslation;
using Apps.Crowdin.Models.Request.ProjectGroups;
using Apps.Crowdin.Models.Response.MachineTranslation;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.MachineTranslationEngines;

namespace Apps.Crowdin.Actions;

[ActionList]
public class MachineTranslationActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("Search machine translation engines", Description = "List all machine translation engines")]
    public async Task<ListMtEnginesResponse> ListMtEnginges(
        [ActionParameter] ProjectGroupRequest projectGroupDataHandler)
    {
        var intGroupId = IntParser.Parse(projectGroupDataHandler.ProjectGroupId, nameof(projectGroupDataHandler.ProjectGroupId));
        
        var items = await Paginator.Paginate((lim, offset)
            => SdkClient.MachineTranslationEngines.ListMts(intGroupId, lim, offset));

        var mtEntities = items.Select(x => new MtEngineEntity(x)).ToArray();
        return new(mtEntities);
    }

    [Action("Translate via machine translation engine", Description = "Translate text via machine translation engine")]
    public async Task<MtTextTranslationEntity> TranslateTextViaMt(
        [ActionParameter] MtEngineRequest mtEngine,
        [ActionParameter] TranslateTextRequest input)
    {
        var response = await TranslateLinesViaMt(mtEngine, new(input));
        return new(response);
    }

    [Action("Translate lines via machine translation engine",
        Description = "Translate multiple text lines via machine translation engine")]
    public async Task<MtStringsTranslationEntity> TranslateLinesViaMt(
        [ActionParameter] MtEngineRequest mtEngine,
        [ActionParameter] TranslateStringsRequest input)
    {
        var intMtId = IntParser.Parse(mtEngine.MtEngineId, nameof(mtEngine.MtEngineId));

        var recognitionProvider =
            EnumParser.Parse<LanguageRecognitionProvider>(input.LanguageRecognitionProvider,
                nameof(input.LanguageRecognitionProvider));

        var request = new TranslateViaMtRequest
        {
            TargetLanguageId = input.TargetLanguageId,
            SourceLanguageId = input.SourceLanguageId,
            Strings = input.Text.ToList(),
            LanguageRecognitionProvider = recognitionProvider
        };

        var response = await SdkClient.MachineTranslationEngines
            .TranslateViaMt(intMtId!.Value, request);
        return new(response);
    }
}