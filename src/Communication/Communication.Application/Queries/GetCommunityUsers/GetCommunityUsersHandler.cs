using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Community.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityUsers;

internal class GetCommunityUsersHandler(ICommunityRepository repository, IMapper mapper) : IRequestHandler<GetCommunityUsersQuery, AllCommunityUserDto>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllCommunityUserDto> Handle(GetCommunityUsersQuery request, CancellationToken cancellationToken)
    {
        var (communityUsers, count) = await _repository.GetCommunityUsersAsync(request.CommunityId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityUserDto>>(communityUsers);

        return new AllCommunityUserDto(map, count);
    }
}
