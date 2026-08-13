namespace Communication.Application.DTOs.Post;

public class CommunityPostCommentDto
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public int CommentType { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public int CommunityId { get; set; }

    public int CommunityPostId { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
