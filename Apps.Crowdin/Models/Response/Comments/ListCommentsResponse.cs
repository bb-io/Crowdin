using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.Comments;

public record ListCommentsResponse(CommentEntity[] Comments);