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
    internal class InviteToGroupChatService : IService<InviteToGroupChatDto, int>
    {
        private readonly IGenericRepository<InviteToGroupChat, int> _repository;
        private readonly IMapper _mapper;

        public InviteToGroupChatService(IGenericRepository<InviteToGroupChat, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<InviteToGroupChatDto> IService<InviteToGroupChatDto, int>.CreateAsync(InviteToGroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(InviteToGroupChatDto), $"The {nameof(InviteToGroupChatDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<InviteToGroupChatDto, int>.DeleteAsync(InviteToGroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(InviteToGroupChatDto), $"The {nameof(InviteToGroupChatDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<InviteToGroupChatDto>> IService<InviteToGroupChatDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<InviteToGroupChatDto>>(allData);

            return result;
        }

        async Task<InviteToGroupChatDto> IService<InviteToGroupChatDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<InviteToGroupChatDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<InviteToGroupChatDto>> IService<InviteToGroupChatDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<InviteToGroupChatDto>>(result);

            return resultMap;
        }

        Task<int> IService<InviteToGroupChatDto, int>.UpdateAsync(InviteToGroupChatDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(InviteToGroupChatDto), $"The {nameof(InviteToGroupChatDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<InviteToGroupChatDto> CreateInternalAsync(InviteToGroupChatDto item)
        {
            var map = _mapper.Map<InviteToGroupChat>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<InviteToGroupChatDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(InviteToGroupChatDto item)
        {
            var map = _mapper.Map<InviteToGroupChat>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(InviteToGroupChatDto item)
        {
            var map = _mapper.Map<InviteToGroupChat>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
