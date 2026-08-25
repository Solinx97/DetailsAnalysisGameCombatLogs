using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetInvitesToCommunity;

internal class GetInvitesToCommunityHandler(IInviteToCommunityRepository repository, IMapper mapper) : IRequestHandler<GetInvitesToCommunityQuery, IEnumerable<InviteToCommunityDto>>
{
    private readonly IInviteToCommunityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<InviteToCommunityDto>> Handle(GetInvitesToCommunityQuery request, CancellationToken cancellationToken)
    {
        var communityInvites = await _repository.GetByUserIdAsync(request.AppUserId, cancellationToken);
        var map = _mapper.Map<IEnumerable<InviteToCommunityDto>>(communityInvites);

        return map;
    }
}

