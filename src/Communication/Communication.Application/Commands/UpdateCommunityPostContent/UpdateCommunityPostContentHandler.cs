using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using Communication.Domain.ValueObjects;
using MediatR;

namespace Communication.Application.Commands.UpdateCommunityPostContent;

internal class UpdateCommunityPostContentHandler(IGenericRepository<CommunityPost, CommunityPostId> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommunityPostContentCommand>
{
    private readonly IGenericRepository<CommunityPost, CommunityPostId> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommunityPostContentCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetByIdAsync(request.Id, cancellationToken);
        userPost.EditContent(request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}