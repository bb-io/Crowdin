﻿using Apps.Crowdin.Invocables;
using Apps.Crowdin.Models.Entities;
using Apps.Crowdin.Models.Request.Comments;
using Apps.Crowdin.Models.Request.Project;
using Apps.Crowdin.Models.Response.Comments;
using Apps.Crowdin.Utils;
using Blackbird.Applications.Sdk.Common;
using Blackbird.Applications.Sdk.Common.Actions;
using Blackbird.Applications.Sdk.Common.Authentication;
using Blackbird.Applications.Sdk.Common.Invocation;
using Blackbird.Applications.Sdk.Utils.Parsers;
using Crowdin.Api.StringComments;

namespace Apps.Crowdin.Actions;

[ActionList]
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
        
        var request = new AddStringCommentRequest
        {
            StringId = intStringId!.Value,
            Text = input.Text,
            TargetLanguageId = input.TargetLanguageId,
            Type = EnumParser.Parse<StringCommentType>(input.Type, nameof(input.Type))!
                .Value,
            IssueType = EnumParser.Parse<IssueType>(input.IssueType, nameof(input.IssueType))
        };

        var response = await ExceptionWrapper.ExecuteWithErrorHandling(async () => 
            await SdkClient.StringComments.AddStringComment(intProjectId!.Value, request));
        return new(response);
    }

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