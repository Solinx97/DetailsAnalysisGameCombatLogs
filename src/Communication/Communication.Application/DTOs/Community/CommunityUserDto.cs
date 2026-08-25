namespace Communication.Application.DTOs.Community;

public class CommunityUserDto
{
    public string Id { get; set; } = string.Empty;

    public int CommunityId { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
