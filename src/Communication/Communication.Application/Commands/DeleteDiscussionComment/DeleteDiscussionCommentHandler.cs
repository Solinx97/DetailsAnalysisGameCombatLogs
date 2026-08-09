using Communication.Application.Commands.DeleteInviteToCommunity;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteDiscussionComment;

internal class DeleteDiscussionCommentHandler(ICommunityDiscussionRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteInviteToCommunityCommand>
{
    private readonly ICommunityDiscussionRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteInviteToCommunityCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetWithCommentsAsync(request.CommunityId, cancellationToken);
        community.RemoveComment(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
