namespace Communication.Application.DTOs.Post;

public class UserPostDislikeDto
{
    public int Id { get; set; }

    public int UserPostId { get; set; }

    public Guid AppUserId { get; set; }

    public int Status { get; set; }
}
