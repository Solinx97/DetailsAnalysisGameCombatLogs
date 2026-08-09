using Communication.Application.Commands.CreateCommunityPostLike;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostDislike;

internal class CreateCommunityPostDislikeHandler(IGenericRepository<CommunityPost, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunityPostDislikeCommand>
{
    private readonly IGenericRepository<CommunityPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateCommunityPostDislikeCommand request, CancellationToken cancelationToken)
    {
        var communityPost = await _repository.GetByIdAsync(request.CommunityPostId, cancelationToken);
        communityPost.AddDislike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
