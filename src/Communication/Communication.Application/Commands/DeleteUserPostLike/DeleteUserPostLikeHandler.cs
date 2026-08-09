using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteUserPostLike;

internal class DeleteUserPostLikeHandler(IUserPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserPostLikeCommand>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteUserPostLikeCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetWithDislikeAsync(request.UserPostId, cancellationToken);
        userPost.RemoveLike(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
