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
    internal class PersonalChatService : IService<PersonalChatDto, int>
    {
        private readonly IGenericRepository<PersonalChat, int> _repository;
        private readonly IMapper _mapper;

        public PersonalChatService(IGenericRepository<PersonalChat, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<PersonalChatDto> IService<PersonalChatDto, int>.CreateAsync(PersonalChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<PersonalChatDto, int>.DeleteAsync(PersonalChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<PersonalChatDto>> IService<PersonalChatDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<PersonalChatDto>>(allData);

            return result;
        }

        async Task<PersonalChatDto> IService<PersonalChatDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<PersonalChatDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<PersonalChatDto>> IService<PersonalChatDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<PersonalChatDto>>(result);

            return resultMap;
        }

        Task<int> IService<PersonalChatDto, int>.UpdateAsync(PersonalChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<PersonalChatDto> CreateInternalAsync(PersonalChatDto item)
        {
            var map = _mapper.Map<PersonalChat>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<PersonalChatDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(PersonalChatDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(PersonalChatDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<PersonalChat>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(PersonalChatDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(PersonalChatDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<PersonalChat>(item));
            return numberEntriesAffected;
        }
    }
}
