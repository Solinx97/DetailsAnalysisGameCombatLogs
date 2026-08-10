using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostComment;

internal class CreateCommunityPostCommentHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunityPostCommentCommand>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateCommunityPostCommentCommand request, CancellationToken cancelationToken)
    {
        var post = await _repository.GetWithCommentsAsync(request.CommunityPostId, cancelationToken);
        post.AddComment(request.Content, request.CommentType, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
