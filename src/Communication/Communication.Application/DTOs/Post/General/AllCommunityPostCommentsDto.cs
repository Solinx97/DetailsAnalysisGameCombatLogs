using Communication.Application.DTOs.Post;

namespace Communication.Application.DTOs.Post.General;

public record AllCommunityPostCommentsDto(
    IEnumerable<CommunityPostCommentDto> Comments,
    int Count
);
