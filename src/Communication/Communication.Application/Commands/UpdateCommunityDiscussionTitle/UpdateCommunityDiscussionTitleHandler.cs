using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.UpdateCommunityDiscussionTitle;

internal class UpdateCommunityDiscussionHandlerr(IGenericRepository<CommunityDiscussion, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommunityDiscussionCommand>
{
    private readonly IGenericRepository<CommunityDiscussion, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommunityDiscussionCommand request, CancellationToken cancellationToken)
    {
        var communityDiscassion = await _repository.GetByIdAsync(request.Id, cancellationToken);
        communityDiscassion.Edit(request.Title, request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}