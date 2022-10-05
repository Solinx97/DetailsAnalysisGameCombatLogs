using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.Chat;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
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
                throw new ArgumentNullException(nameof(PersonalChatMessageDto), $"The {nameof(PersonalChatMessageDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<PersonalChatMessageDto, int>.DeleteAsync(PersonalChatMessageDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(PersonalChatMessageDto), $"The {nameof(PersonalChatMessageDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<PersonalChatMessageDto>> IService<PersonalChatMessageDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<PersonalChatMessageDto>>(allData);

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
                throw new ArgumentNullException(nameof(PersonalChatMessageDto), $"The {nameof(PersonalChatMessageDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<PersonalChatMessageDto> CreateInternalAsync(PersonalChatMessageDto item)
        {
            if (string.IsNullOrEmpty(item.Username))
            {
                throw new ArgumentNullException(nameof(PersonalChatMessageDto),
                    $"The property {nameof(PersonalChatMessageDto.Username)} of the {nameof(PersonalChatMessageDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Message))
            {
                throw new ArgumentNullException(nameof(PersonalChatMessageDto), 
                    $"The property {nameof(PersonalChatMessageDto.Message)} of the {nameof(PersonalChatMessageDto)} object can't be null or empty");
            }

            var map = _mapper.Map<PersonalChatMessage>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<PersonalChatMessageDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(PersonalChatMessageDto item)
        {
            if (string.IsNullOrEmpty(item.Username))
            {
                throw new ArgumentNullException(nameof(PersonalChatMessageDto), 
                    $"The property {nameof(PersonalChatMessageDto.Username)} of the {nameof(PersonalChatMessageDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Message))
            {
                throw new ArgumentNullException(nameof(PersonalChatMessageDto), 
                    $"The property {nameof(PersonalChatMessageDto.Message)} of the {nameof(PersonalChatMessageDto)} object can't be null or empty");
            }

            var map = _mapper.Map<PersonalChatMessage>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(PersonalChatMessageDto item)
        {
            if (string.IsNullOrEmpty(item.Username))
            {
                throw new ArgumentNullException(nameof(PersonalChatMessageDto), 
                    $"The property {nameof(PersonalChatMessageDto.Username)} of the {nameof(PersonalChatMessageDto)} object can't be null or empty");
            }
            if (string.IsNullOrEmpty(item.Message))
            {
                throw new ArgumentNullException(nameof(PersonalChatMessageDto), 
                    $"The property {nameof(PersonalChatMessageDto.Message)} of the {nameof(PersonalChatMessageDto)} object can't be null or empty");
            }

            var map = _mapper.Map<PersonalChatMessage>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
