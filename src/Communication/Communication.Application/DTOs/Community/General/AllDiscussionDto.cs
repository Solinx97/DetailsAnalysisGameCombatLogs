namespace Communication.Application.DTOs.Community.General;

public record AllDiscussionDto(
    IEnumerable<CommunityDiscussionDto> Discussions,
    int Count
);
