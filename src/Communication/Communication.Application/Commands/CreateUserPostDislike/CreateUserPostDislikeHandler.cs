using Communication.Application.Commands.CreateUserPostLike;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostDislike;

internal class CreateUserPostDislikeHandler(IGenericRepository<UserPost, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserPostLikeCommand>
{
    private readonly IGenericRepository<UserPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateUserPostLikeCommand request, CancellationToken cancelationToken)
    {
        var userPost = await _repository.GetByIdAsync(request.UserPostId, cancelationToken);
        userPost.AddDislike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}

