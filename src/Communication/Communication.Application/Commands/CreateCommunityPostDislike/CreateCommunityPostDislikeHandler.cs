using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostDislike;

internal class CreateCommunityPostDislikeHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunityPostDislikeCommand>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateCommunityPostDislikeCommand request, CancellationToken cancelationToken)
    {
        var communityPost = await _repository.GetWithReactionsAsync(request.CommunityPostId, cancelationToken);
        communityPost.AddDislike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
