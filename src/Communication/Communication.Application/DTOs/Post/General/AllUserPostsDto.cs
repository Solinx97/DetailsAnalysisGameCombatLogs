namespace Communication.Application.DTOs.Post.General;

public record AllUserPostsDto(
    IEnumerable<UserPostDto> Posts,
    int Count
    );
