using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Application.DTOs.Post.General;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Queries.GetUserPostByUserId;

internal class GetUserPostByUserIdHandler(IUserPostRepository repository, IMapper mapper) : IRequestHandler<GetUserPostByUserIdQuery, AllUserPostsDto>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<AllUserPostsDto> Handle(GetUserPostByUserIdQuery request, CancellationToken cancellationToken)
    {
        var (communities, count) = await _repository.GetByUserIdAsync(request.AppUserId, request.Page, request.PageSize, cancellationToken);
        var map = _mapper.Map<IEnumerable<UserPostDto>>(communities);

        return new AllUserPostsDto(map, count);
    }
}
