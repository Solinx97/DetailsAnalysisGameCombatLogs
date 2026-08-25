using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteCommunityPost;

internal class DeleteCommunityPostHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommunityPostCommand>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCommunityPostCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

