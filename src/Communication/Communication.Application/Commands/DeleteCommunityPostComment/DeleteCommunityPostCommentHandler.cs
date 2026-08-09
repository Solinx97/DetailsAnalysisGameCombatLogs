using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteCommunityPostComment;

internal class DeleteCommunityPostCommentHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommunityPostCommentCommand>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCommunityPostCommentCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetWithCommentsAsync(request.CommunityPostId, cancellationToken);
        post.RemoveComment(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
