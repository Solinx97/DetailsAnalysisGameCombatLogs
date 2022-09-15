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
    public class InviteToGroupChatController : ControllerBase
    {
        private readonly IService<InviteToGroupChatDto, int> _service;
        private readonly IMapper _mapper;

        public InviteToGroupChatController(IService<InviteToGroupChatDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<InviteToGroupChatModel>> Get()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<InviteToGroupChatModel>>(result);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<InviteToGroupChatModel> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<InviteToGroupChatModel>(result);

            return map;
        }

        [HttpPost]
        public async Task<InviteToGroupChatModel> Create(InviteToGroupChatModel model)
        {
            var map = _mapper.Map<InviteToGroupChatDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<InviteToGroupChatModel>(result);

            return resultMap;
        }

        [HttpPut]
        public async Task<int> Update(InviteToGroupChatModel model)
        {
            var map = _mapper.Map<InviteToGroupChatDto>(model);
            var result = await _service.UpdateAsync(map);

            return result;
        }

        [HttpDelete]
        public async Task<int> Delete(InviteToGroupChatModel model)
        {
            var map = _mapper.Map<InviteToGroupChatDto>(model);
            var result = await _service.DeleteAsync(map);

            return result;
        }
    }
}
