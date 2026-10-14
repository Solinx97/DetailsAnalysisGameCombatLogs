namespace Communication.Application.DTOs.Community;

public class InviteToCommunityDto
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public Guid ToAppUserId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public Guid AppUserId { get; set; }
}
