namespace Communication.Application.DTOs.Community;

public class CommunityDiscussionDto
{
    public int Id { get; set; }

    public string Title { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public int CommunityId { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
