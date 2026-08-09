using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateDiscussionComment;

internal class CreateDiscussionCommentHandler(IGenericRepository<CommunityDiscussion, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateDiscussionCommentCommand>
{
    private readonly IGenericRepository<CommunityDiscussion, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateDiscussionCommentCommand request, CancellationToken cancelationToken)
    {
        var discussion = await _repository.GetByIdAsync(request.CommunityDiscussionId, cancelationToken);
        discussion.AddComment(request.Content, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
