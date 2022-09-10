using AutoMapper;
using CombatAnalysis.BL.DTO.User;
using CombatAnalysis.BL.Exceptions;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.DAL.Entities.User;
using CombatAnalysis.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.BL.Services
{
    internal class UserService : IUserService<UserDto>
    {
        private readonly IUserRepository _repository;
        private readonly IMapper _mapper;

        public UserService(IUserRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<string> IUserService<UserDto>.CreateAsync(UserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IUserService<UserDto>.DeleteAsync(UserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<UserDto>> IUserService<UserDto>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<UserDto>>(allData);

            return result;
        }

        async Task<UserDto> IUserService<UserDto>.GetByIdAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<UserDto>(user);

            return result;
        }

        async Task<UserDto> IUserService<UserDto>.GetAsync(string emil, string password)
        {
            var user = await _repository.GetAsync(emil, password);
            var result = _mapper.Map<UserDto>(user);

            return result;
        }

        async Task<UserDto> IUserService<UserDto>.GetAsync(string emil)
        {
            var user = await _repository.GetAsync(emil);
            var result = _mapper.Map<UserDto>(user);

            return result;
        }

        Task<int> IUserService<UserDto>.UpdateAsync(UserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }


        private async Task<string> CreateInternalAsync(UserDto item)
        {
            var map = _mapper.Map<User>(item);
            var createdUserId = await _repository.CreateAsync(map);

            return createdUserId;
        }

        private async Task<int> DeleteInternalAsync(UserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(UserDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(_mapper.Map<User>(item));
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(UserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(UserDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.UpdateAsync(_mapper.Map<User>(item));
            return numberEntries;
        }
    }
}
