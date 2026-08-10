using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteUserPostComment;

internal class DeleteUserPostCommentHandler(IUserPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserPostCommentCommand>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteUserPostCommentCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetWithCommentsAsync(request.UserPostId, cancellationToken);
        userPost.RemoveComment(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}