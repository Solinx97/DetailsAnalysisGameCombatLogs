namespace Communication.Application.DTOs.Community.General;

public record AllCommunityDto(
    IEnumerable<CommunityDto> Communities,
    int Count
);
