using Apps.Crowdin.Api.RestSharp;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Common;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common.Actions;
using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Request.Users;
using Apps.Crowdin.Models.Response;
using Crowdin.Api;
using Newtonsoft.Json;
using RestSharp;
using Apps.Crowdin.Models.Response.Project;
using Blackbird.Applications.Sdk.Utils.Parsers;

namespace Apps.Crowdin.Actions;

[ActionList("Users")]
public class UserActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("Search project members", Description = "Get all matching project members")]
    public async Task<SearchUsersResponse> SearchProjectMembers([ActionParameter] SearchUsersRequest input)
    {
        var items = await Paginator.Paginate(async (lim, offset)
            =>
        {
            var request =
                new CrowdinRestRequest(
                    $"/projects/{input.ProjectId}/members?limit={lim}&offset={offset}",
                    Method.Get, Creds);
            if (input.Role != null)
            {
                request.AddQueryParameter("role", input.Role);
            }

            if (input.LanguageId != null)
            {
                request.AddQueryParameter("languageId", input.LanguageId);
            }

            var response = await RestClient.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ResponseList<DataResponse<AssigneeEntity>>>(response.Content);
        });

        var users = items.Select(x => x.Data).ToArray();
        return new SearchUsersResponse(users);
    }

    [Action("Find project member", Description = "Get first matching project member")]
    public async Task<AssigneeEntity?> FindProjectUsers([ActionParameter] FindUserRequest input)
    {
        var items = await Paginator.Paginate(async (lim, offset)
            =>
        {
            var request =
                new CrowdinRestRequest(
                    $"/projects/{input.ProjectId}/members?limit={lim}&offset={offset}",
                    Method.Get, Creds);
            if (input.Role != null)
            {
                request.AddQueryParameter("role", input.Role);
            }

            if (input.LanguageId != null)
            {
                request.AddQueryParameter("languageId", input.LanguageId);
            }

            if (input.search != null)
            {
                request.AddQueryParameter("search", input.search);
            }

            var response = await RestClient.ExecuteAsync(request);
            return JsonConvert.DeserializeObject<ResponseList<DataResponse<AssigneeEntity>>>(response.Content);
        });

        var users = items.Select(x => x.Data).ToArray();
        return users.FirstOrDefault();
    }

    [Action("Invite user", Description = "Add a new user")]
    public async Task<UserEnterpriseEntity> InviteUser(
        [ActionParameter] InviteUserRequest input)
    {
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Users.InviteUser(new()
        {
            Email = input.Email,
            FirstName = input.FirstName,
            LastName = input.LastName,
            TimeZone = input.Timezone,
            AdminAccess = input.IsAdmin
        }));

        return new(response);
    }

    [Action("Delete user", Description = "Delete specific user")]
    public async Task DeleteUser(
        [ActionParameter] UserRequest user)
    {
        await ExceptionWrapper.ExecuteWithErrorHandling(async () =>
            await SdkClient.Users.DeleteUser(IntParser.Parse(user.UserId, nameof(user.UserId))!.Value));
    }
}