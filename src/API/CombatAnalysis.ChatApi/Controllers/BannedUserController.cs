using AutoMapper;
using CombatAnalysis.BL.DTO.Chat;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.ChatApi.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CombatAnalysis.ChatApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class BannedUserController : ControllerBase
    {
        private readonly IService<BannedUserDto, int> _service;
        private readonly IMapper _mapper;

        public BannedUserController(IService<BannedUserDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<BannedUserModel>> Get()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<BannedUserModel>>(result);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<BannedUserModel> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<BannedUserModel>(result);

            return map;
        }

        [HttpPost]
        public async Task<BannedUserModel> Create(BannedUserModel model)
        {
            var map = _mapper.Map<BannedUserDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<BannedUserModel>(result);

            return resultMap;
        }

        [HttpPut]
        public async Task<int> Update(BannedUserModel model)
        {
            var map = _mapper.Map<BannedUserDto>(model);
            var result = await _service.UpdateAsync(map);

            return result;
        }

        [HttpDelete]
        public async Task<int> Delete(BannedUserModel model)
        {
            var map = _mapper.Map<BannedUserDto>(model);
            var result = await _service.DeleteAsync(map);

            return result;
        }
    }
}
