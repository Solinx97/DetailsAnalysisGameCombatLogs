using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteCommunityPostLike;

internal class DeleteCommunityPostLikeHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommunityPostLikeCommand>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCommunityPostLikeCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetWithLikeAsync(request.CommunityPostId, cancellationToken);
        userPost.RemoveLike(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
