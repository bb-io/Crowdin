using Apps.Crowdin.Api;
using Apps.Crowdin.Constants;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Comments;
using Apps.Crowdin.Models.Response.Comments;
using Apps.Crowdin.Utils;
using Apps.Crowdin.Utils.Parsers;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Crowdin.Api.StringComments;

namespace Apps.Crowdin.Actions;

[ActionList]
public class CommentActions
{
    [Action("List comments", Description = "List string comments for a project")]
    public async Task<ListCommentsResponse> ListComments(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] ListCommentsRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

        var client = new CrowdinClient(creds);

        var items = await Paginator.Paginate((lim, offset)
            =>
        {
            var request = new StringCommentsListParams
            {
                Limit = lim,
                Offset = offset,
                StringId = intStringId,
                Type =
                    EnumParser.Parse<StringCommentType>(input.Type, nameof(input.Type), EnumValues.StringCommentType),
                IssueStatus = EnumParser.Parse<IssueStatus>(input.IssueStatus, nameof(input.IssueStatus),
                    EnumValues.IssueStatus),
                IssueTypes = input.IssueTypes?.Select(issueType =>
                        EnumParser.Parse<IssueType>(issueType, nameof(issueType), EnumValues.IssueType)!.Value)
                    .ToHashSet() ?? new()
            };
            return client.StringComments.ListStringComments(intProjectId!.Value, request);
        });

        var comments = items.Select(x => new CommentEntity(x)).ToArray();
        return new(comments);
    }
    
    [Action("Get string comment", Description = "Get specific string comment")]
    public async Task<CommentEntity> GetStringComment(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId,
        [ActionParameter] [Display("Comment ID")] string commentId)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intCommentId = IntParser.Parse(commentId, nameof(commentId));

        var client = new CrowdinClient(creds);
       
        var response = await client.StringComments.GetStringComment(intProjectId!.Value, intCommentId!.Value);
        return new(response);
    }
    
    [Action("Add string comment", Description = "Add new string comment")]
    public async Task<CommentEntity> AddComment(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] AddNewCommentRequest input)
    {
        var intProjectId = IntParser.Parse(input.ProjectId, nameof(input.ProjectId));
        var intStringId = IntParser.Parse(input.StringId, nameof(input.StringId));

        var client = new CrowdinClient(creds);

        var request = new AddStringCommentRequest
        {
            StringId = intStringId!.Value,
            Text = input.Text,
            TargetLanguageId = input.TargetLanguageId,
            Type = EnumParser.Parse<StringCommentType>(input.Type, nameof(input.Type), EnumValues.StringCommentType)!.Value,
            IssueType = EnumParser.Parse<IssueType>(input.IssueType, nameof(input.IssueType), EnumValues.IssueType)
        };
        
        var response = await client.StringComments.AddStringComment(intProjectId!.Value, request);
        return new(response);
    }
    
    [Action("Delete string comment", Description = "Delete specific string comment")]
    public Task DeleteComment(
        IEnumerable<AuthenticationCredentialsProvider> creds,
        [ActionParameter] [Display("Project ID")] string projectId,
        [ActionParameter] [Display("Comment ID")] string commentId)
    {
        var intProjectId = IntParser.Parse(projectId, nameof(projectId));
        var intCommentId = IntParser.Parse(commentId, nameof(commentId));

        var client = new CrowdinClient(creds);

        return client.StringComments.DeleteStringComment(intProjectId!.Value, intCommentId!.Value);
    }
}