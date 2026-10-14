using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using MediatR;

namespace Communication.Application.Commands.UpdateUserPostContent;

internal class UpdateUserPostContentHandler(IGenericRepository<UserPost, UserPostId> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateUserPostContentCommand>
{
    private readonly IGenericRepository<UserPost, UserPostId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateUserPostContentCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetByIdAsync(request.Id, cancellationToken);
        userPost.EditContent(request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
