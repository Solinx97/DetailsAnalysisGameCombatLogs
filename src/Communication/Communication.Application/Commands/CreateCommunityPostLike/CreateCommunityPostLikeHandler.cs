using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostLike;

internal class CreateCommunityPostLikeHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunityPostLikeCommand>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateCommunityPostLikeCommand request, CancellationToken cancelationToken)
    {
        var communityPost = await _repository.GetWithReactionsAsync(request.CommunityPostId, cancelationToken);
        communityPost.AddLike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
