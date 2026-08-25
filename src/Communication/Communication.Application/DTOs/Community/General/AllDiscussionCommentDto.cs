using Communication.Application.DTOs.Community;

namespace Communication.Application.DTOs.Community.General;

public record AllDiscussionCommentDto(
    IEnumerable<CommunityDiscussionCommentDto> Comments,
    int Count
);
