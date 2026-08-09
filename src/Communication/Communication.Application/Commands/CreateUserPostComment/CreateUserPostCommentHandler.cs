using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostComment;

internal class CreateUserPostCommentHandler(IGenericRepository<UserPost, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<CreateUserPostCommentCommand>
{
    private readonly IGenericRepository<UserPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(CreateUserPostCommentCommand request, CancellationToken cancelationToken)
    {
        var userPost = await _repository.GetByIdAsync(request.UserPostId, cancelationToken);
        userPost.AddComment(request.Content, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);
    }
}

