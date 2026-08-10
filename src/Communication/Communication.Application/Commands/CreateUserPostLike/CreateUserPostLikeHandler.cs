using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostLike;

internal class CreateUserPostLikeHandler(IUserPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserPostLikeCommand>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateUserPostLikeCommand request, CancellationToken cancelationToken)
    {
        var userPost = await _repository.GetWithReactionsAsync(request.UserPostId, cancelationToken);
        userPost.AddLike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
