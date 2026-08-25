using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Application.DTOs.Post.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityPost;

internal class GetCommunityPostHandler(ICommunityPostRepository repository, IMapper mapper) : IRequestHandler<GetCommunityPostQuery, AllCommunityPostsDto>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllCommunityPostsDto> Handle(GetCommunityPostQuery request, CancellationToken cancellationToken)
    {
        var (posts, count) = await _repository.GetByCommunityIdAsync(request.CommunityId, request.AppUserId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(posts);

        return new AllCommunityPostsDto(map, count);
    }
}
