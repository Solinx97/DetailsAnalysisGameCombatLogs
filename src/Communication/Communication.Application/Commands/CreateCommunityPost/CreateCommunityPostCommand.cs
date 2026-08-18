using Communication.Application.DTOs.Post;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPost;

public record CreateCommunityPostCommand(
    string Content,
    int PostType,
    int PublicType,
    int Restrictions,
    string Tags,
    int CommunityId,
    string AppUserId
    ) : IRequest<CommunityPostDto>;