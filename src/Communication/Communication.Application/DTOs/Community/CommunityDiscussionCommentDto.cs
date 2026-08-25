namespace Communication.Application.DTOs.Community;

public class CommunityDiscussionCommentDto
{
    public int Id { get; set; }

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public int CommunityDiscussionId { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
