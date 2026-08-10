namespace Communication.Application.DTOs.Post;

public class CommunityPostDto
{
    public int Id { get; set; }

    public string CommunityName { get; set; } = string.Empty;

    public string Owner { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int PostType { get; set; }

    public int PublicType { get; set; }

    public int Restrictions { get; set; }

    public string Tags { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public int LikeCount { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
