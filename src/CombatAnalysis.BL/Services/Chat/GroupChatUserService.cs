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
    internal class GroupChatUserService : IService<GroupChatUserDto, int>
    {
        private readonly IGenericRepository<GroupChatUser, int> _repository;
        private readonly IMapper _mapper;

        public GroupChatUserService(IGenericRepository<GroupChatUser, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<GroupChatUserDto> IService<GroupChatUserDto, int>.CreateAsync(GroupChatUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<GroupChatUserDto, int>.DeleteAsync(GroupChatUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<GroupChatUserDto>> IService<GroupChatUserDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<GroupChatUserDto>>(allData);

            return result;
        }

        async Task<GroupChatUserDto> IService<GroupChatUserDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<GroupChatUserDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<GroupChatUserDto>> IService<GroupChatUserDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<GroupChatUserDto>>(result);

            return resultMap;
        }

        Task<int> IService<GroupChatUserDto, int>.UpdateAsync(GroupChatUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(GroupChatUserDto), $"The {nameof(GroupChatUserDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<GroupChatUserDto> CreateInternalAsync(GroupChatUserDto item)
        {
            var map = _mapper.Map<GroupChatUser>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<GroupChatUserDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(GroupChatUserDto item)
        {
            var map = _mapper.Map<GroupChatUser>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(GroupChatUserDto item)
        {
            var map = _mapper.Map<GroupChatUser>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
