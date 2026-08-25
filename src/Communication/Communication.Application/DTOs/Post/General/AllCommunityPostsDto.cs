namespace Communication.Application.DTOs.Post.General;

public record AllCommunityPostsDto(
    IEnumerable<CommunityPostDto> Posts,
    int Count
    );
