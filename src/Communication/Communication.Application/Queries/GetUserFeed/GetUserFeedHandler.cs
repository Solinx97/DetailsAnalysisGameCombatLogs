using AutoMapper;
using Communication.Application.DTOs.Post.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetUserFeed;

internal class GetUserFeedHandler(IUserFeedRepository repository, IMapper mapper) : IRequestHandler<GetUserFeedQuery, AllUserFeedDto>
{
    private readonly IUserFeedRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllUserFeedDto> Handle(GetUserFeedQuery request, CancellationToken cancellationToken)
    {
        var (feed, count) = await _repository.GetUserFeedAsync(request.AppUserId, request.FriendsId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<UserFeedDto>>(feed);

        return new AllUserFeedDto(map, count);
    }
}

