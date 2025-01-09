using Apps.Crowdin.Models.Entities;

namespace Apps.Crowdin.Models.Response.ReviewedFile;

public record ListReviewedFileBuildsResponse(ReviewedFileBuildEntity[] Builds);