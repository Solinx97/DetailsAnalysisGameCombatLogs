using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.UpdateUserPostCommentContent;

internal class UpdateUserPostCommentContentHandler(IUserPostRepository repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserPostCommentContentCommand>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateUserPostCommentContentCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetWithCommentsAsync(request.UserPostId, cancellationToken);
        userPost.EditCommentContent(request.Id, request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
