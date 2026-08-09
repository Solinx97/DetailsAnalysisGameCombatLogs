using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostLike;

internal class CreateCommunityPostLikeHandler(IGenericRepository<CommunityPost, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunityPostLikeCommand>
{
    private readonly IGenericRepository<CommunityPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateCommunityPostLikeCommand request, CancellationToken cancelationToken)
    {
        var communityPost = await _repository.GetByIdAsync(request.CommunityPostId, cancelationToken);
        communityPost.AddLike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
