using AutoMapper;
using Communication.Application.DTOs.Post;
using Communication.Domain.Aggregates;
using Communication.Domain.Data;
using MediatR;

namespace Communication.Application.Commands.CreateUserPost;

internal class CreateUserPostHandler(IGenericRepository<UserPost, int> repository, IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<CreateUserPostCommand, UserPostDto>
{
    private readonly IGenericRepository<UserPost, int> _repository = repository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IMapper _mapper = mapper;

    public async Task<UserPostDto> Handle(CreateUserPostCommand request, CancellationToken cancelationToken)
    {
        var community = UserPost.Create(request.Owner, request.Content, request.PublicType, request.Tags, 0, 0, 0, request.AppUserId);
        await _repository.AddAsync(community, cancelationToken);

        await _unitOfWork.SaveChangesAsync(cancelationToken);

        var map = _mapper.Map<UserPostDto>(community);

        return map;
    }
}
