namespace Communication.Application.DTOs.Community.General;

public record AllCommunityUserDto(
    IEnumerable<CommunityUserDto> Users,
    int Count
);
