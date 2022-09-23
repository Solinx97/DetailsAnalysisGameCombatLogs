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
    internal class PersonalChatMessageService : IService<PersonalChatMessageDto, int>
    {
        private readonly IGenericRepository<PersonalChatMessage, int> _repository;
        private readonly IMapper _mapper;

        public PersonalChatMessageService(IGenericRepository<PersonalChatMessage, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<PersonalChatMessageDto> IService<PersonalChatMessageDto, int>.CreateAsync(PersonalChatMessageDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<PersonalChatMessageDto, int>.DeleteAsync(PersonalChatMessageDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<PersonalChatMessageDto>> IService<PersonalChatMessageDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<PersonalChatMessageDto>>(allData);

            return result;
        }

        async Task<PersonalChatMessageDto> IService<PersonalChatMessageDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<PersonalChatMessageDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<PersonalChatMessageDto>> IService<PersonalChatMessageDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(result);

            return resultMap;
        }

        Task<int> IService<PersonalChatMessageDto, int>.UpdateAsync(PersonalChatMessageDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<PersonalChatMessageDto> CreateInternalAsync(PersonalChatMessageDto item)
        {
            var map = _mapper.Map<PersonalChatMessage>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<PersonalChatMessageDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(PersonalChatMessageDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(PersonalChatMessageDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<PersonalChatMessage>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(PersonalChatMessageDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(PersonalChatMessageDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<PersonalChatMessage>(item));
            return numberEntriesAffected;
        }
    }
}
