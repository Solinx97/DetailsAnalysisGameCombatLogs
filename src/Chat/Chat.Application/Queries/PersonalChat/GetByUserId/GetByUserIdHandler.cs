using AutoMapper;
using Chat.Application.DTOs;
using Chat.Domain.Repositories;
using MediatR;

namespace Chat.Application.Queries.PersonalChat.GetByUserId;

internal class GetByUserIdHandler(IPersonalChatRepository repository, IMapper mapper) : IRequestHandler<GetByUserIdQuery, IEnumerable<PersonalChatDto>>
{
    private readonly IPersonalChatRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<IEnumerable<PersonalChatDto>> Handle(GetByUserIdQuery request, CancellationToken cancellationToken)
    {
        var chats = await _repository.GetByUserIdAsync(request.UserId, cancellationToken);
        var map = _mapper.Map<IEnumerable<PersonalChatDto>>(chats);

        return map;
    }
}
