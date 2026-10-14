namespace Communication.Application.DTOs.Post;

public class UserPostCommentDto
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public int UserPostId { get; set; }

    public Guid AppUserId { get; set; }
}