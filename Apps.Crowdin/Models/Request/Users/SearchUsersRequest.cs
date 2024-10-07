using Apps.Crowdin.DataSourceHandlers;
using Apps.Crowdin.DataSourceHandlers.EnumHandlers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Dictionaries;
using Blackbird.Applications.Sdk.Common.Dynamic;

namespace Apps.Crowdin.Models.Request.Users
{
    public class SearchUsersRequest
    {
        [Display("Project")]
        [DataSource(typeof(ProjectDataHandler))]
        public string ProjectId { get; set; }

        [Display("Language ID")]
        [DataSource(typeof(LanguagesDataHandler))]
        public string? LanguageId { get; set; }

        [StaticDataSource(typeof(UserRoleHandler))]
        public string? Role { get; set; }

    }
}
