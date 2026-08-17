using AutoMapper;
using Communication.Application.DTOs.Community;
using Communication.Application.DTOs.Community.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunitiesByUserId;

internal class GetCommunitiesByUserIdHandler(ICommunityRepository repository, IMapper mapper) : IRequestHandler<GetCommunitiesByUserIdQuery, AllCommunityDto>
{
    private readonly ICommunityRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllCommunityDto> Handle(GetCommunitiesByUserIdQuery request, CancellationToken cancellationToken)
    {
        var (communities, count) = await _repository.GetByUserIdAsync(request.AppUserId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityDto>>(communities);

        return new AllCommunityDto(map, count);
    }
}
