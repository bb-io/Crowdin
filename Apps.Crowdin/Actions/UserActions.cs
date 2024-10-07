using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Models.Request.Users;
using Apps.Crowdin.Models.Response;
using Crowdin.Api;
using Newtonsoft.Json;
using RestSharp;
using Apps.Crowdin.Models.Response.Project;

namespace Apps.Crowdin.Actions;

    [ActionList]
    public class UserActions : BaseInvocable
    {
        private AuthenticationCredentialsProvider[] Creds =>
            InvocationContext.AuthenticationCredentialsProviders.ToArray();

        public UserActions(InvocationContext invocationContext) : base(invocationContext)
        {
        }

        [Action("Search project members", Description = "Get all matching project members")]
        public async Task<SearchUsersResponse> SearchProjectMembers([ActionParameter] SearchUsersRequest input)
        {

            var client = new CrowdinRestClient();
            var items = await Paginator.Paginate(async (lim, offset)
                =>
            {
                var request =
                new CrowdinRestRequest(
                        $"/projects/{input.ProjectId}/members?limit={lim}&offset={offset}",
                        Method.Get, Creds);
                if (input.Role != null)
                { request.AddQueryParameter("role", input.Role); }
                if (input.LanguageId != null)
                { request.AddQueryParameter("languageId", input.LanguageId); }
                var response = await client.ExecuteAsync(request);
                return JsonConvert.DeserializeObject<ResponseList<DataResponse<AssigneeEntity>>>(response.Content);
            });

            var users = items.Select(x => x.Data).ToArray();
            return new SearchUsersResponse(users) ;
        }

    [Action("Find project member", Description = "Get first matching project member")]
    public async Task<AssigneeEntity> FindProjectUsers([ActionParameter] FindUserRequest input)
    {

        var client = new CrowdinRestClient();
        var items = await Paginator.Paginate(async (lim, offset)
            =>
        {
            var request =
            new CrowdinRestRequest(
                    $"/projects/{input.ProjectId}/members?limit={lim}&offset={offset}",
                    Method.Get, Creds);
            if (input.Role != null)
            { request.AddQueryParameter("role", input.Role); }
            if (input.LanguageId != null)
            { request.AddQueryParameter("languageId", input.LanguageId); }
            if (input.search != null)
            { request.AddQueryParameter("search", input.search); }
            var response = await client.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ResponseList<DataResponse<AssigneeEntity>>>(response.Content);
        });

        var users = items.Select(x => x.Data).ToArray();
        return users.FirstOrDefault();
    }

}

