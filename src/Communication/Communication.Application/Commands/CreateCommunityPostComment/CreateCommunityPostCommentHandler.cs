using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityPostComment;

internal class CreateCommunityPostCommentHandler(IGenericRepository<CommunityPost, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateCommunityPostCommentCommand>
{
    private readonly IGenericRepository<CommunityPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateCommunityPostCommentCommand request, CancellationToken cancelationToken)
    {
        var post = await _repository.GetByIdAsync(request.CommunityPostId, cancelationToken);
        post.AddComment(request.Content, request.CommentType, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}
