using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateDiscussionComment;

internal class CreateDiscussionCommentHandler(IGenericRepository<CommunityDiscussion, int> repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateDiscussionCommentCommand, CommunityDiscussionCommentDto>
{
    private readonly IGenericRepository<CommunityDiscussion, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityDiscussionCommentDto> Handle(CreateDiscussionCommentCommand request, CancellationToken cancelationToken)
    {
        var discussion = await _repository.GetByIdAsync(request.CommunityDiscussionId, cancelationToken);
        var comment = discussion.AddComment(request.Content, request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<CommunityDiscussionCommentDto>(comment);

        return map;
    }
}
