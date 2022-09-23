using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Exceptions;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services.Chat
{
    internal class GroupChatMessageService : IService<GroupChatMessageDto, int>
    {
        private readonly IGenericRepository<GroupChatMessage, int> _repository;
        private readonly IMapper _mapper;

        public GroupChatMessageService(IGenericRepository<GroupChatMessage, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<GroupChatMessageDto> IService<GroupChatMessageDto, int>.CreateAsync(GroupChatMessageDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<GroupChatMessageDto, int>.DeleteAsync(GroupChatMessageDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<GroupChatMessageDto>> IService<GroupChatMessageDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<GroupChatMessageDto>>(allData);

            return result;
        }

        async Task<GroupChatMessageDto> IService<GroupChatMessageDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<GroupChatMessageDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<GroupChatMessageDto>> IService<GroupChatMessageDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<GroupChatMessageDto>>(result);

            return resultMap;
        }

        Task<int> IService<GroupChatMessageDto, int>.UpdateAsync(GroupChatMessageDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<GroupChatMessageDto> CreateInternalAsync(GroupChatMessageDto item)
        {
            var map = _mapper.Map<GroupChatMessage>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<GroupChatMessageDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(GroupChatMessageDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(GroupChatMessageDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<GroupChatMessage>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(GroupChatMessageDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(GroupChatMessageDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<GroupChatMessage>(item));
            return numberEntriesAffected;
        }
    }
}
