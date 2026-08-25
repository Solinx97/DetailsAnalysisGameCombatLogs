using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.UpdateCommunityPostCommentContent;

internal class UpdateCommunityPostCommentContentHandler(ICommunityPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommunityPostCommentContentCommand>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommunityPostCommentContentCommand request, CancellationToken cancellationToken)
    {
        var post = await _repository.GetWithCommentsAsync(request.CommunityPostId, cancellationToken);
        post.EditCommentContent(request.Id, request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
