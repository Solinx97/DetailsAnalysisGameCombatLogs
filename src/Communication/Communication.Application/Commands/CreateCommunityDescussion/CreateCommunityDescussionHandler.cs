using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateCommunityDescussion;

internal class CreateCommunityDescussionHandler(IGenericRepository<CommunityDiscussion, int> repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateCommunityDescussionCommand, CommunityDiscussionDto>
{
    private readonly IGenericRepository<CommunityDiscussion, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<CommunityDiscussionDto> Handle(CreateCommunityDescussionCommand request, CancellationToken cancelationToken)
    {
        var communityDiscussion = CommunityDiscussion.Create(request.Title, request.Content, request.CommunityId, request.AppUserId);
        await _repository.AddAsync(communityDiscussion, cancelationToken);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<CommunityDiscussionDto>(communityDiscussion);

        return map;
    }
}

