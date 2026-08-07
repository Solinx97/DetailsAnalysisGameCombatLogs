namespace Communication.Application.DTOs.Community;

public class InviteToCommunityDto
{
    public int Id { get; set; }

    public int CommunityId { get; set; }

    public string ToAppUserId { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
