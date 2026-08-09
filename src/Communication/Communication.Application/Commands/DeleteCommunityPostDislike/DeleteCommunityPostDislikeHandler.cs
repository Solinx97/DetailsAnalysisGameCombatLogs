using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteCommunityPostDislike;

internal class DeleteCommunityPostDislikeHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommunityPostDislikeCommand>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCommunityPostDislikeCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetWithLikeAsync(request.CommunityPostId, cancellationToken);
        userPost.RemoveDislike(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
