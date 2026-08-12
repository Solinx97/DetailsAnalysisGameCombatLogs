using AutoMapper;
using Communication.Application.DTOs.Post.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetUserFeed;

internal class GetUserFeedHandler(IUserFeedRepository repository, IMapper mapper) : IRequestHandler<GetUserFeedQuery, IEnumerable<UserFeedDto>>
{
    private readonly IUserFeedRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UserFeedDto>> Handle(GetUserFeedQuery request, CancellationToken cancellationToken)
    {
        var feed = await _repository.GetUserFeedAsync(request.AppUserId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<UserFeedDto>>(feed);

        return map;
    }
}

