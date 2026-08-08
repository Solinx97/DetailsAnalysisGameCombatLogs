using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityUsers;

internal class GetCommunityUsersHandler(ICommunityRepository repository, IMapper mapper) : IRequestHandler<GetCommunityUsersQuery, IEnumerable<CommunityUserDto>>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommunityUserDto>> Handle(GetCommunityUsersQuery request, CancellationToken cancellationToken)
    {
        var communityUsers = await _repository.GetCommunityUsersAsync(request.CommunityId, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityUserDto>>(communityUsers);

        return map;
    }
}
