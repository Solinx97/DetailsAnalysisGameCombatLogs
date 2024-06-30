using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.Identity.Services;

internal class ClientService : IClientService
{
    private readonly IClientRepository _clientRepository;
    private readonly IMapper _mapper;

    public ClientService(IClientRepository clientRepository, IMapper mapper)
    {
        _clientRepository = clientRepository;
        _mapper = mapper;
    }

    public async Task CreateAsync(ClientDto user)
    {
        var map = _mapper.Map<Client>(user);

        await _clientRepository.SaveAsync(map);
    }

    public async Task<ClientDto> GetAsymc(string id)
    {
        var identityUser = await _clientRepository.GetByIdAsync(id);
        var map = _mapper.Map<ClientDto>(identityUser);

        return map;
    }
}
