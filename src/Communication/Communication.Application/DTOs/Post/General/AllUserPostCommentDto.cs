namespace Communication.Application.DTOs.Post.General;

public record AllUserPostCommentDto(
    IEnumerable<UserPostCommentDto> Comments,
    int Count
);