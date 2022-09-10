using AutoMapper;
using CombatAnalysis.StoredProcedureBL.DTO.User;
using CombatAnalysis.StoredProcedureBL.Exceptions;
using CombatAnalysis.StoredProcedureBL.Interfaces;
using CombatAnalysis.StoredProcedureDAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CombatAnalysis.StoredProcedureBL.Services
{
    internal class UserService : IService<UserDto, string>
    {
        private readonly IGenericRepository<UserDto, string> _repository;
        private readonly IMapper _mapper;

        public UserService(IGenericRepository<UserDto, string> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        Task<int> IService<UserDto, string>.CreateAsync(UserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return CreateInternalAsync(item);
        }

        Task<int> IService<UserDto, string>.DeleteAsync(UserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return DeleteInternalAsync(item);
        }

        async Task<IEnumerable<UserDto>> IService<UserDto, string>.GetAllAsync()
        {
            var allData = await _repository.GetAllAsync();
            var result = _mapper.Map<List<UserDto>>(allData);

            return result;
        }

        async Task<UserDto> IService<UserDto, string>.GetByIdAsync(string id)
        {
            var user = await _repository.GetByIdAsync(id);
            var result = _mapper.Map<UserDto>(user);

            return result;
        }

        //async Task<UserDto> IService<UserDto, string>.GetAsync(string emil, string password)
        //{
        //    var user = await _repository.GetAsync(emil, password);
        //    var result = _mapper.Map<UserDto>(user);

        //    return result;
        //}

        //async Task<UserDto> IService<UserDto, string>.GetAsync(string emil)
        //{
        //    var user = await _repository.GetAsync(emil);
        //    var result = _mapper.Map<UserDto>(user);

        //    return result;
        //}

        Task<int> IService<UserDto, string>.UpdateAsync(UserDto item)
        {
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item));
            }

            return UpdateInternalAsync(item);
        }


        private async Task<int> CreateInternalAsync(UserDto item)
        {
            var paramNames = new string[] { nameof(item.Email), nameof(item.Password) };
            var paramValues = new object[] { item.Email, item.Password };

            var data = await _repository.CreateAsync(paramNames, paramValues);

            return data;
        }

        private async Task<int> DeleteInternalAsync(UserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(UserDto)} not found", nameof(allData));
            }

            var numberEntries = await _repository.DeleteAsync(item.Id);
            return numberEntries;
        }

        private async Task<int> UpdateInternalAsync(UserDto item)
        {
            var allData = await _repository.GetAllAsync();
            if (!allData.Any())
            {
                throw new NotFoundException($"Collection entity {nameof(UserDto)} not found", nameof(allData));
            }

            var paramNames = new string[] { nameof(item.Email), nameof(item.Password) };
            var paramValues = new object[] { item.Email, item.Password };

            var numberEntries = await _repository.UpdateAsync(paramNames, paramValues);
            return numberEntries;
        }
    }
}
