using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteUserPost;

internal class DeleteUserPostHandler(IUserPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteUserPostCommand>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteUserPostCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
