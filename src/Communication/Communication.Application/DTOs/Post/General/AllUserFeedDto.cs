namespace Communication.Application.DTOs.Post.General;

public record AllUserFeedDto(
    IEnumerable<UserFeedDto> Posts,
    int Count
    );
