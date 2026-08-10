using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetUserPostByUserId;

internal class GetUserPostByUserIdHandler(IUserPostRepository repository, IMapper mapper) : IRequestHandler<GetUserPostByUserIdQuery, IEnumerable<UserPostDto>>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<UserPostDto>> Handle(GetUserPostByUserIdQuery request, CancellationToken cancellationToken)
    {
        var communities = await _repository.GetByUserIdAsync(request.AppUserId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(communities);

        return map;
    }
}
