using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteCommunityDiscussion;

internal class DeleteCommunityDiscussionHandler(ICommunityDiscussionRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteCommunityDiscussionComand>
{
    private readonly ICommunityDiscussionRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteCommunityDiscussionComand request, CancellationToken cancellationToken)
    {
        await _repository.DeleteAsync(request.Id, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
