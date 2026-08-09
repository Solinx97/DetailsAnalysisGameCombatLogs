using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostLike;

internal class CreateUserPostLikeHandler(IGenericRepository<UserPost, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserPostLikeCommand>
{
    private readonly IGenericRepository<UserPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateUserPostLikeCommand request, CancellationToken cancelationToken)
    {
        var userPost = await _repository.GetByIdAsync(request.UserPostId, cancelationToken);
        userPost.AddLike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
