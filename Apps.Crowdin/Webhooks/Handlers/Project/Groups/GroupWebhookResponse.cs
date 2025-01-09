using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Webhooks.Models.Payload;
using Apps.Crowdin.Webhooks.Models.Payload.Project.Wrappers;

namespace Apps.Crowdin.Webhooks.Handlers.Project.Groups;

public class GroupWebhookResponse : CrowdinWebhookResponse<GroupWrapper>
{
    public GroupEntity Group { get; set; }

    public UserEntity User { get; set; }

    public override void ConfigureResponse(GroupWrapper wrapper)
    {
        Group = new(wrapper.Group);
        User = new(wrapper.User);
    }
}