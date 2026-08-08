using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.UpdateCommunityDiscussionTitle;

internal class UpdateCommunityDiscussionTitleHandlerr(IGenericRepository<CommunityDiscussion, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommunityDiscussionTitleCommand>
{
    private readonly IGenericRepository<CommunityDiscussion, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommunityDiscussionTitleCommand request, CancellationToken cancellationToken)
    {
        var communityDiscassion = await _repository.GetByIdAsync(request.Id, cancellationToken);
        communityDiscassion.EditTile(request.Title);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}