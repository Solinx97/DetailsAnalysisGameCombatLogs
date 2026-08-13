using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostLike;

internal class CreateUserPostLikeHandler(IUserPostRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateUserPostLikeCommand, UserPostLikeDto>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UserPostLikeDto> Handle(CreateUserPostLikeCommand request, CancellationToken cancellationToken)
    {
        var userPost = await _repository.GetWithReactionsAsync(request.UserPostId, cancellationToken);
        var (like, status) = userPost.AddLike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var map = _mapper.Map<UserPostLikeDto>(like);
        map.Status = (int)status;

        return map;
    }
}
