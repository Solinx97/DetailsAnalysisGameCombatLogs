using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.DeleteDiscussionComment;

internal class DeleteDiscussionCommentHandler(ICommunityDiscussionRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<DeleteDiscussionCommentCommand>
{
    private readonly ICommunityDiscussionRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(DeleteDiscussionCommentCommand request, CancellationToken cancellationToken)
    {
        var community = await _repository.GetWithCommentsAsync(request.DiscussionId, cancellationToken);
        community.RemoveComment(request.Id);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
