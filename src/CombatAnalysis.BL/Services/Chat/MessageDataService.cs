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
    internal class MessageDataService : IService<MessageDataDto, int>
    {
        private readonly IGenericRepository<MessageData, int> _repository;
        private readonly IMapper _mapper;

        public MessageDataService(IGenericRepository<MessageData, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<MessageDataDto> IService<MessageDataDto, int>.CreateAsync(MessageDataDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<MessageDataDto, int>.DeleteAsync(MessageDataDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<MessageDataDto>> IService<MessageDataDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<MessageDataDto>>(allData);

            return result;
        }

        async Task<MessageDataDto> IService<MessageDataDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<MessageDataDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<MessageDataDto>> IService<MessageDataDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<MessageDataDto>>(result);

            return resultMap;
        }

        Task<int> IService<MessageDataDto, int>.UpdateAsync(MessageDataDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }

        private async Task<MessageDataDto> CreateInternalAsync(MessageDataDto item)
        {
            var map = _mapper.Map<MessageData>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<MessageDataDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(MessageDataDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(MessageDataDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.DeleteAsync(_mapper.Map<MessageData>(item));
            return numberEntriesAffected;
        }

        private async Task<int> UpdateInternalAsync(MessageDataDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(MessageDataDto)} not found", nameof(allData));
            }

            var numberEntriesAffected = await _repository.UpdateAsync(_mapper.Map<MessageData>(item));
            return numberEntriesAffected;
        }
    }
}
