using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteCommunity;

internal class DeleteCommunityHandler(ICommunityRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommunityCommand>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}

