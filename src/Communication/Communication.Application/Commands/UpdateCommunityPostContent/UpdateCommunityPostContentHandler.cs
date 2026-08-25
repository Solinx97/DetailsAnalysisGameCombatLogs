using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.UpdateCommunityPostContent;

internal class UpdateCommunityPostContentHandler(IGenericRepository<CommunityPost, int> repository, IUnitOfWork unitOfWork) : IRequestHandler<UpdateCommunityPostContentCommand>
{
    private readonly IGenericRepository<CommunityPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task Handle(UpdateCommunityPostContentCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetByIdAsync(request.Id, cancellationToken);
        userPost.EditContent(request.Content);

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}