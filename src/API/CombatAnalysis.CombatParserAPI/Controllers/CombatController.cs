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
    public class CombatController : ControllerBase
    {
        private readonly IService<CombatDto> _service;
        private readonly IMapper _mapper;

        public CombatController(IService<CombatDto> service, IMapper mapper)
        {
            _service = service;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IEnumerable<CombatModel>> Get()
        {
            var combats = await _service.GetAllAsync();
            var map = _mapper.Map<IEnumerable<CombatModel>>(combats);

            return map;
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post(CombatDto value)
        {
            var map = _mapper.Map<CombatDto>(value);
            _service.CreateAsync(map);
        }

        [HttpPut("{id}")]
        public void Put(int id, string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
