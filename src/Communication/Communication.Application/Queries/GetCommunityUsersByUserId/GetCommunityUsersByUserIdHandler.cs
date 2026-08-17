using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Community.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityUsersByUserId;

internal class GetCommunityUsersByUserIdHandler(ICommunityRepository repository, IMapper mapper) : IRequestHandler<GetCommunityUsersByUserIdQuery, AllCommunityUserDto>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllCommunityUserDto> Handle(GetCommunityUsersByUserIdQuery request, CancellationToken cancellationToken)
    {
        var (communityUsers, count) = await _repository.GetCommunityUsersByUserIdAsync(request.AppUserId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityUserDto>>(communityUsers);

        return new AllCommunityUserDto(map, count);
    }
}
