using Crowdin.Api.MachineTranslationEngines;

namespace Apps.Crowdin.Models.Entities;

public class MtEngineEntity
{
    public string Id { get; set; }

    public string GroupId { get; set; }

    public string Name { get; set; }

    public string Type { get; set; }

    public MtEngineEntity(MtEngine mtEngine)
    {
        Id = mtEngine.Id.ToString();
        GroupId = mtEngine.GroupId.ToString();
        Name = mtEngine.Name;
        Type = mtEngine.Type;
    }
}