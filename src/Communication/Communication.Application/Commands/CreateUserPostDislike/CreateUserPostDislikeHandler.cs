using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostDislike;

internal class CreateUserPostDislikeHandler(IUserPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserPostDislikeCommand>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateUserPostDislikeCommand request, CancellationToken cancelationToken)
    {
        var userPost = await _repository.GetWithReactionsAsync(request.UserPostId, cancelationToken);
        userPost.AddDislike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}

