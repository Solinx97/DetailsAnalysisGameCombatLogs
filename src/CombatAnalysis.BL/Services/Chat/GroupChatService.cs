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
    internal class GroupChatService : IService<GroupChatDto, int>
    {
        private readonly IGenericRepository<GroupChat, int> _repository;
        private readonly IMapper _mapper;

        public GroupChatService(IGenericRepository<GroupChat, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<GroupChatDto> IService<GroupChatDto, int>.CreateAsync(GroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<GroupChatDto, int>.DeleteAsync(GroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<GroupChatDto>> IService<GroupChatDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<GroupChatDto>>(allData);

            return result;
        }

        async Task<GroupChatDto> IService<GroupChatDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<GroupChatDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<GroupChatDto>> IService<GroupChatDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<GroupChatDto>>(result);

            return resultMap;
        }

        Task<int> IService<GroupChatDto, int>.UpdateAsync(GroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<GroupChatDto> CreateInternalAsync(GroupChatDto item)
        {
            var map = _mapper.Map<GroupChat>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<GroupChatDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(GroupChatDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(GroupChatDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<GroupChat>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(GroupChatDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(GroupChatDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<GroupChat>(item));
            return numberEntriesAffected;
        }
    }
}
