using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetUserPost;

internal class GetUserPostHandler(IGenericRepository<UserPost, int> repository, IMapper mapper) : IRequestHandler<GetUserPostQuery, IEnumerable<UserPostDto>>
{
    private readonly IGenericRepository<UserPost, int> _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UserPostDto>> Handle(GetUserPostQuery request, CancellationToken cancellationToken)
    {
        var communities = await _repository.GetAsync(request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(communities);

        return map;
    }
}
