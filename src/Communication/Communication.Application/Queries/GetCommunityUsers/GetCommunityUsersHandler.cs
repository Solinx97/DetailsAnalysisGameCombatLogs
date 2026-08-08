using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityUsers;

internal class GetCommunityUsersHandler(ICommunityUserRepository repository, IMapper mapper) : IRequestHandler<GetCommunityUsersQuery, IEnumerable<CommunityUserDto>>
{
    private readonly ICommunityUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommunityUserDto>> Handle(GetCommunityUsersQuery request, CancellationToken cancellationToken)
    {
        var communities = await _repository.GetByCommunityIdAsync(request.CommunityId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityUserDto>>(communities);

        return map;
    }
}
