namespace Communication.Application.DTOs.Community;

public class CommunityDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public int PolicyType { get; set; }

    public string AppUserId { get; set; } = string.Empty;
}
