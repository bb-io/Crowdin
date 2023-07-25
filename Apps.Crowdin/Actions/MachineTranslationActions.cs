using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.MachineTranslation;
using Apps.Crowdin.Models.Response.MachineTranslation;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Utils.Parsers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api.MachineTranslationEngines;

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
    public async Task<MtTextTranslationEntity> TranslateTextViaMt(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Machine translation engine ID")]
        string mtEngineId,
        [ActionParameter] TranslateTextRequest input)
    {
        var response = await TranslateLinesViaMt(creds, mtEngineId, new(input));
        return new(response);
    }

    [Action("Translate lines via machine translation engine",
        Description = "Translate multiple text lines via machine translation engine")]
    public async Task<MtStringsTranslationEntity> TranslateLinesViaMt(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Machine translation engine ID")]
        string mtEngineId,
        [ActionParameter] TranslateStringsRequest input)
    {
        var intMtId = IntParser.Parse(mtEngineId, nameof(mtEngineId));

        var recognitionProvider =
            EnumParser.Parse<LanguageRecognitionProvider>(input.LanguageRecognitionProvider,
                nameof(input.LanguageRecognitionProvider),
                EnumValues.LanguageRecognitionProvider);

        var client = new CrowdinClient(creds);

        var request = new TranslateViaMtRequest
        {
            TargetLanguageId = input.TargetLanguageId,
            SourceLanguageId = input.SourceLanguageId,
            Strings = input.Text.ToList(),
            LanguageRecognitionProvider = recognitionProvider
        };

        var response = await client.MachineTranslationEngines
            .TranslateViaMt(intMtId!.Value, request);
        return new(response);
    }
}