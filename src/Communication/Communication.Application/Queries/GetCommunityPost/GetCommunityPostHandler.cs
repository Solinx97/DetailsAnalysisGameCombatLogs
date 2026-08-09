using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetCommunityPost;

internal class GetCommunityPostHandler(ICommunityPostRepository repository, IMapper mapper) : IRequestHandler<GetCommunityPostQuery, IEnumerable<CommunityPostDto>>
{
    private readonly ICommunityPostRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<CommunityPostDto>> Handle(GetCommunityPostQuery request, CancellationToken cancellationToken)
    {
        var communityPosts = await _repository.GetByCommunityIdAsync(request.CommunityId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<CommunityPostDto>>(communityPosts);

        return map;
    }
}
