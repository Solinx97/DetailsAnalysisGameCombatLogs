using AutoMapper;
using Chat.Application.DTOs;
using Chat.Application.Interfaces;
using Chat.Application.Mappers;
using Chat.Domain.Aggregates;
using Chat.Domain.Entities;
using Chat.Domain.Exceptions;
using Chat.Domain.Repositories;
using Chat.Domain.ValueObjects;

namespace Chat.Application.Services;

internal class GroupChatUserService(IGenericRepository<GroupChat, GroupChatId> chatRepository, IGroupChatUserRepository repository, IMapper mapper)
    : IGroupChatUserService
{
    private readonly IGenericRepository<GroupChat, GroupChatId> _chatRepository = chatRepository;
    private readonly IGroupChatUserRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

    public async Task<GroupChatUserDto> CreateAsync(GroupChatUserDto createUser)
    {
        var chat = await _chatRepository.GetByIdAsync(createUser.GroupChatId)
                            ?? throw new GroupChatNotFoundException(createUser.GroupChatId);

        var groupChatUser = GroupChatUser.Create(createUser.Username, createUser.AppUserId, 0);

        var createdUser = await _repository.CreateAsync(groupChatUser);

        return createdUser.ToDTO(_mapper);
    }

    public async Task UpdateChatUserAsync(GroupChatUserId id,
                                GroupChatMessageId? lastMessageId = null, 
                                int? unreadMessages = null)
    {
        var entity = await _repository.GetByIdAsync(id);

        if (lastMessageId != null)
        {
            entity.MarkAsRead(lastMessageId.Value);
        }

        if (unreadMessages != null && unreadMessages > 0)
        {
            entity.UpdateUnreadMessages(unreadMessages.Value);
        }

        await _repository.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
    }

    public async Task<IEnumerable<GroupChatUserDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();

        return allData.ToDTOCollection(_mapper);
    }

    public async Task<GroupChatUserDto> GetByIdAsync(Guid id)
    {
        var result = await _repository.GetByIdAsync(id) 
                ?? throw new GroupChatUserNotFoundException(id);

        return result.ToDTO(_mapper);
    }

    public async Task<IEnumerable<GroupChatUserDto>> FindAllAsync(int chatId)
    {
        var users = await _repository.FindAllAsync(chatId);

        return users.ToDTOCollection(_mapper);
    }

    public async Task<GroupChatUserDto> FindByAppUserIdAsync(int chatId, Guid appUserId)
    {
        var user = await _repository.FindByAppUserIdAsync(chatId, appUserId)
                ?? throw new GroupChatUserNotFoundException(appUserId);

        return user.ToDTO(_mapper);
    }

    public async Task<IEnumerable<GroupChatUserDto>> FindAllByAppUserIdAsync(Guid appUserId)
    {
        var users = await _repository.FindAllByAppUserIdAsync(appUserId);

        return users.ToDTOCollection(_mapper);
    }
}
