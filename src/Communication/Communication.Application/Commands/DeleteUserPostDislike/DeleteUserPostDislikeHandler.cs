using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteUserPostDislike;

internal class DeleteUserPostDislikeHandler(IUserPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserPostDislikeCommand>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteUserPostDislikeCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetWithDislikeAsync(request.UserPostId, cancellationToken);
        userPost.RemoveDislike(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
