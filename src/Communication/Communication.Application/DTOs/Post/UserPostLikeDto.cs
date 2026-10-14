namespace Communication.Application.DTOs.Post;

public class UserPostLikeDto
{
    public int Id { get; set; }

    public int UserPostId { get; set; }

    public Guid AppUserId { get; set; }

    public int Status { get; set; }
}
