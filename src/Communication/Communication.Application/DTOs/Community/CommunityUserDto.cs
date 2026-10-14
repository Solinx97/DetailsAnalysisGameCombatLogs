namespace Communication.Application.DTOs.Community;

public class CommunityUserDto
{
    public Guid Id { get; set; }

    public int CommunityId { get; set; }

    public Guid AppUserId { get; set; }
}
