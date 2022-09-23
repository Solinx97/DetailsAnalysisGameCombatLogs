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
    public class GroupChatController : ControllerBase
    {
        private readonly IService<GroupChatDto, int> _service;
        private readonly IMapper _mapper;

        public GroupChatController(IService<GroupChatDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<GroupChatModel>> Get()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<GroupChatModel>>(result);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<GroupChatModel> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<GroupChatModel>(result);

            return map;
        }

        [HttpPost]
        public async Task<GroupChatModel> Create(GroupChatModel model)
        {
            var map = _mapper.Map<GroupChatDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<GroupChatModel>(result);

            return resultMap;
        }

        [HttpPut]
        public async Task<int> Update(GroupChatModel model)
        {
            var map = _mapper.Map<GroupChatDto>(model);
            var result = await _service.UpdateAsync(map);

            return result;
        }

        [HttpDelete]
        public async Task<int> Delete(GroupChatModel model)
        {
            var map = _mapper.Map<GroupChatDto>(model);
            var result = await _service.DeleteAsync(map);

            return result;
        }
    }
}
