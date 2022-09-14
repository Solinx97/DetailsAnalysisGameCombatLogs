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
    public class MessageDataController : ControllerBase
    {
        private readonly IService<MessageDataDto, int> _service;
        private readonly IMapper _mapper;

        public MessageDataController(IService<MessageDataDto, int> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<MessageDataModel>> Get()
        {
            var result = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<MessageDataModel>>(result);

            return map;
        }

        [HttpGet("{id:int:min(1)}")]
        public async Task<MessageDataModel> GetById(int id)
        {
            var result = await _service.GetByIdAsync(id);
            var map = _mapper.Map<MessageDataModel>(result);

            return map;
        }

        [HttpPost]
        public async Task<MessageDataModel> Create(MessageDataModel model)
        {
            var map = _mapper.Map<MessageDataDto>(model);
            var result = await _service.CreateAsync(map);
            var resultMap = _mapper.Map<MessageDataModel>(result);

            return resultMap;
        }

        [HttpPut]
        public async Task<int> Update(MessageDataModel model)
        {
            var map = _mapper.Map<MessageDataDto>(model);
            var result = await _service.UpdateAsync(map);

            return result;
        }

        [HttpDelete]
        public async Task<int> Delete(MessageDataModel model)
        {
            var map = _mapper.Map<MessageDataDto>(model);
            var result = await _service.DeleteAsync(map);

            return result;
        }
    }
}
