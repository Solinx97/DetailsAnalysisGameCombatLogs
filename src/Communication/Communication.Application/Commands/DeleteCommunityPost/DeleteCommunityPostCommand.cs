using MediatR;

namespace Communication.Application.Commands.DeleteCommunityPost;

public record DeleteCommunityPostCommand(
    int Id
    ) : IRequest;
