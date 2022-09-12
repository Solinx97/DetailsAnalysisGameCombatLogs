using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.CombatParserAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CombatLogByUserController : ControllerBase
    {
        private readonly IService<CombatLogByUserDto, int> _service;
        private readonly IMapper _mapper;

        public CombatLogByUserController(IService<CombatLogByUserDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CombatLogByUserModel>> Get()
        {
            var combatLogsByUser = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<CombatLogByUserModel>>(combatLogsByUser);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<CombatLogByUserModel> GetById(int id)
        {
            var combatLogByUser = await _service.GetByIdAsync(id);
            var map = _mapper.Map<CombatLogByUserModel>(combatLogByUser);

            return map;
        }

        [HttpGet("byUserId/{userId}")]
        public async Task<IEnumerable<CombatLogByUserModel>> GetByUserId(string userId)
        {
            var combatLogsByUser = new List<CombatLogByUserModel>();
            var allCombatLogsByUser = await _service.GetAllAsync();
            var mapAllCombatLogsByUser = _mapper.Map<IEnumerable<CombatLogByUserModel>>(allCombatLogsByUser);
            foreach (var item in mapAllCombatLogsByUser)
            {
                if (item.UserId == userId)
                {
                    combatLogsByUser.Add(item);
                }
            }

            return combatLogsByUser;
        }

        [HttpPost]
        public async Task<CombatLogByUserModel> Post(CombatLogByUserModel model)
        {
            var map = _mapper.Map<CombatLogByUserDto>(model);
            var createdItem = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<CombatLogByUserModel>(createdItem);

            return resultMap;
        }

        [HttpPut]
        public async Task Put(CombatLogByUserModel value)
        {
            var map = _mapper.Map<CombatLogByUserDto>(value);
            await _service.UpdateAsync(map);
        }

        [HttpDelete]
        public async Task<int> Delete(CombatLogByUserModel value)
        {
            var map = _mapper.Map<CombatLogByUserDto>(value);
            var deletedId = await _service.DeleteAsync(map);

            return deletedId;
        }
    }
}
