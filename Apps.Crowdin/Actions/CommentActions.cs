using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Api.RestSharp;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Request.Comments;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response;
using Apps.Crowdin.Models.Response.Comments;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Exceptions;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.StringComments;
using RestSharp;

namespace Apps.Crowdin.Actions;

[ActionList("Comments")]
public class CommentActions(InvocationContext invocationContext) : AppInvocable(invocationContext)
{
    [Action("Search comments", Description = "List string comments for a project")]
    public async Task<ListCommentsResponse> ListComments([ActionParameter] ListCommentsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));
        
        var items = await Paginator.Paginate((lim, offset)
            =>
        {
            var request = new StringCommentsListParams
            {
                Limit = lim,
                Offset = offset,
                StringId = intStringId,
                Type =
                    EnumParser.Parse<StringCommentType>(input.Type, nameof(input.Type)),
                IssueStatus = EnumParser.Parse<IssueStatus>(input.IssueStatus, nameof(input.IssueStatus)),
                IssueTypes = input.IssueTypes?.Select(issueType =>
                        EnumParser.Parse<IssueType>(issueType, nameof(issueType))!.Value)
                    .ToHashSet() ?? new()
            };
            
            return ExceptionWrapper.ExecuteWithErrorHandling(
                () => SdkClient.StringComments.ListStringComments(intProjectId!.Value, request));
        });

        var comments = items.Select(x => new CommentEntity(x)).ToArray();
        return new(comments);
    }

    [Action("Get string comment", Description = "Get specific string comment")]
    public async Task<CommentEntity> GetStringComment(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Comment ID")]
        string commentId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intCommentId = IntParser.Parse(commentId, nameof(commentId));
        
        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.StringComments.GetStringComment(intProjectId!.Value, intCommentId!.Value));
        return new(response);
    }

    [Action("Add string comment", Description = "Add new string comment")]
    public async Task<CommentEntity> AddComment([ActionParameter] AddNewCommentRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

        var isEnterprise = Creds.GetCrowdinPlan() == Plans.Enterprise;
        if (input.ShareWithVendor == true && !isEnterprise)
        {
            throw new PluginMisconfigurationException(
                "The 'Share with vendor' input is available only for Crowdin Enterprise connections. " +
                "Leave it empty or use a Crowdin Enterprise connection.");
        }

        var type = EnumParser.Parse<StringCommentType>(input.Type, nameof(input.Type))!.Value;
        var issueType = EnumParser.Parse<IssueType>(input.IssueType, nameof(input.IssueType));

        var requestBody = new Dictionary<string, object>
        {
            ["stringId"] = intStringId!.Value,
            ["text"] = input.Text,
            ["targetLanguageId"] = input.TargetLanguageId,
            ["type"] = ToApiValue(type)
        };

        if (issueType.HasValue)
            requestBody["issueType"] = ToApiValue(issueType.Value);

        if (input.ShareWithVendor.HasValue && isEnterprise)
            requestBody["isShared"] = input.ShareWithVendor.Value;

        var request = new CrowdinRestRequest($"/projects/{intProjectId!.Value}/comments", Method.Post, Creds)
            .AddJsonBody(requestBody);

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(() =>
            RestClient.ExecuteWithErrorHandling<DataResponse<StringComment>>(request));
        return new(response.Data);
    }

    private static string ToApiValue(StringCommentType type) => type switch
    {
        StringCommentType.Comment => "comment",
        StringCommentType.Issue => "issue",
        _ => throw new PluginMisconfigurationException($"Unsupported string comment type: {type}")
    };

    private static string ToApiValue(IssueType issueType) => issueType switch
    {
        IssueType.GeneralQuestion => "general_question",
        IssueType.TranslationMistake => "translation_mistake",
        IssueType.ContextRequest => "context_request",
        IssueType.SourceMistake => "source_mistake",
        _ => throw new PluginMisconfigurationException($"Unsupported issue type: {issueType}")
    };

    [Action("Delete string comment", Description = "Delete specific string comment")]
    public async Task DeleteComment(
        [ActionParameter] ProjectRequest project,
        [ActionParameter] [Display("Comment ID")]
        string commentId)
    {
        var intProjectId = IntParser.Parse(project.ProjectId, nameof(project.ProjectId));
        var intCommentId = IntParser.Parse(commentId, nameof(commentId));
        
        await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.StringComments.DeleteStringComment(intProjectId!.Value, intCommentId!.Value));
    }
}
