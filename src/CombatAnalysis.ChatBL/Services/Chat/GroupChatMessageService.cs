﻿using AutoMapper;
using CombatAnalysis.ChatBL.DTO;
using CombatAnalysis.ChatBL.Interfaces;
using CombatAnalysis.ChatDAL.Entities;
using CombatAnalysis.ChatDAL.Interfaces;

namespace CombatAnalysis.ChatBL.Services.Chat;
internal class GroupChatMessageService : IService<GroupChatMessageDto, int>
{
    private readonly IGenericRepository<GroupChatMessage, int> _repository;
    private readonly IMapper _mapper;

    public GroupChatMessageService(IGenericRepository<GroupChatMessage, int> repository, IMapper mapper)
    {
        _repository = repository;
        _mapper = mapper;
    }

    public Task<GroupChatMessageDto> CreateAsync(GroupChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), $"The {nameof(GroupChatMessageDto)} can't be null");
        }

        return CreateInternalAsync(item);
    }

    public async Task<int> DeleteAsync(int id)
    {
        var rowsAffected = await _repository.DeleteAsync(id);

        return rowsAffected;
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetAllAsync()
    {
        var allData = await _repository.GetAllAsync();
        var result = _mapper.Map<IEnumerable<GroupChatMessageDto>>(allData);

        return result;
    }

    public async Task<GroupChatMessageDto> GetByIdAsync(int id)
    {
        var result = await _repository.GetByIdAsync(id);
        var resultMap = _mapper.Map<GroupChatMessageDto>(result);

        return resultMap;
    }

    public async Task<IEnumerable<GroupChatMessageDto>> GetByParamAsync(string paramName, object value)
    {
        var result = await Task.Run(() => _repository.GetByParam(paramName, value));
        var resultMap = _mapper.Map<IEnumerable<GroupChatMessageDto>>(result);

        return resultMap;
    }

    public Task<int> UpdateAsync(GroupChatMessageDto item)
    {
        if (item == null)
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), $"The {nameof(GroupChatMessageDto)} can't be null");
        }

        return UpdateInternalAsync(item);
    }

    private async Task<GroupChatMessageDto> CreateInternalAsync(GroupChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), 
                $"The property {nameof(GroupChatMessageDto.Message)} of the {nameof(GroupChatMessageDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatMessage>(item);
        var createdItem = await _repository.CreateAsync(map);
        var resultMap = _mapper.Map<GroupChatMessageDto>(createdItem);

        return resultMap;
    }

    private async Task<int> UpdateInternalAsync(GroupChatMessageDto item)
    {
        if (string.IsNullOrEmpty(item.Message))
        {
            throw new ArgumentNullException(nameof(GroupChatMessageDto), 
                $"The property {nameof(GroupChatMessageDto.Message)} of the {nameof(GroupChatMessageDto)} object can't be null or empty");
        }

        var map = _mapper.Map<GroupChatMessage>(item);
        var rowsAffected = await _repository.UpdateAsync(map);

        return rowsAffected;
    }
}
