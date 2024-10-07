using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;
using System.ComponentModel;

namespace Apps.Crowdin.Models.Request.Users
{
    public class FindUserRequest : SearchUsersRequest
    {
        [Display("Name"), Description("Search by username, first name or last name")]
        public string? search { get; set; }

    }
}
