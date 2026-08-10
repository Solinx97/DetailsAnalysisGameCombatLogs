namespace Communication.Application.DTOs.Post;

public class UserPostDto
{
    public int Id { get; set; }

    public string Owner { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public int PublicType { get; set; }

    public string Tags { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
