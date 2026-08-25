using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.UpdateDiscussionCommentContent;

internal class UpdateDiscussionCommentContentHandler(ICommunityDiscussionRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateDiscussionCommentContentCommand>
{
    private readonly ICommunityDiscussionRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateDiscussionCommentContentCommand request, CancellationToken cancellationToken)
    {
        var communityDiscassion = await _repository.GetWithCommentsAsync(request.DiscussionId, cancellationToken);
        communityDiscassion.EditCommentContent(request.Id, request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
