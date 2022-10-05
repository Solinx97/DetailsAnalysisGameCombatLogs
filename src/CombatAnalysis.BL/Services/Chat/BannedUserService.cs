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
    internal class BannedUserService : IService<BannedUserDto, int>
    {
        private readonly IGenericRepository<BannedUser, int> _repository;
        private readonly IMapper _mapper;

        public BannedUserService(IGenericRepository<BannedUser, int> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<BannedUserDto> IService<BannedUserDto, int>.CreateAsync(BannedUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(BannedUserDto), $"The {nameof(BannedUserDto)} can't be null");
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<BannedUserDto, int>.DeleteAsync(BannedUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(BannedUserDto), $"The {nameof(BannedUserDto)} can't be null");
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<BannedUserDto>> IService<BannedUserDto, int>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<IEnumerable<BannedUserDto>>(allData);

            return result;
        }

        async Task<BannedUserDto> IService<BannedUserDto, int>.GetByIdAsync(int id)
        {
            var result = await _repository.GetByIdAsync(id);
            var resultMap = _mapper.Map<BannedUserDto>(result);

            return resultMap;
        }

        async Task<IEnumerable<BannedUserDto>> IService<BannedUserDto, int>.GetByParamAsync(string paramName, object value)
        {
            var result = await Task.Run(() => _repository.GetByParam(paramName, value));
            var resultMap = _mapper.Map<IEnumerable<BannedUserDto>>(result);

            return resultMap;
        }

        Task<int> IService<BannedUserDto, int>.UpdateAsync(BannedUserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(BannedUserDto), $"The {nameof(BannedUserDto)} can't be null");
            }

            return UpdateInternalAsync(item);
        }

        private async Task<BannedUserDto> CreateInternalAsync(BannedUserDto item)
        {
            var map = _mapper.Map<BannedUser>(item);
            var createdItem = await _repository.CreateAsync(map);
            var resultMap = _mapper.Map<BannedUserDto>(createdItem);

            return resultMap;
        }

        private async Task<int> DeleteInternalAsync(BannedUserDto item)
        {
            var map = _mapper.Map<BannedUser>(item);
            var rowsAffected = await _repository.DeleteAsync(map);

            return rowsAffected;
        }

        private async Task<int> UpdateInternalAsync(BannedUserDto item)
        {
            var map = _mapper.Map<BannedUser>(item);
            var rowsAffected = await _repository.UpdateAsync(map);

            return rowsAffected;
        }
    }
}
