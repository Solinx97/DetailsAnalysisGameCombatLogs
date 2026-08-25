using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPostDislike;

internal class CreateUserPostDislikeHandler(IUserPostRepository repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateUserPostDislikeCommand, UserPostDislikeDto>
{
    private readonly IUserPostRepository _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UserPostDislikeDto> Handle(CreateUserPostDislikeCommand request, CancellationToken cancelationToken)
    {
        var userPost = await _repository.GetWithReactionsAsync(request.UserPostId, cancelationToken);
        var (dislike, status) = userPost.AddDislike(request.AppUserId);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<UserPostDislikeDto>(dislike);
        map.Status = (int)status;

        return map;
    }
}

