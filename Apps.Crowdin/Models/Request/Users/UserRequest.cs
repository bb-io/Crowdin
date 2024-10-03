using Apps.Crowdin.DataSourceHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Users
{
    public class UserRequest
    {
        [Display("User ID")]
        [DataSource(typeof(ProjectMemberDataSourceHandler))]
        public string? UserId { get; set; }
    }
}
