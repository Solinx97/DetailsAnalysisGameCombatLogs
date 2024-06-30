using AutoMapper;
using CombatAnalysis.Identity.DTO;
using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.IdentityDAL.Entities;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.Identity.Services;

internal class IdentityUserService : IIdentityUserService
{
    private readonly IIdentityUserRepository _identityUserRepository;
    private readonly IMapper _mapper;

    public IdentityUserService(IIdentityUserRepository identityUserRepository, IMapper mapper)
    {
        _identityUserRepository = identityUserRepository;
        _mapper = mapper;
    }

    public async Task CreateAsync(IdentityUserDto user)
    {
        var map = _mapper.Map<IdentityUser>(user);

        await _identityUserRepository.SaveAsync(map);
    }

    public IdentityUserDto Get(string id)
    {
        var identityUser = _identityUserRepository.Get(id);
        var map = _mapper.Map<IdentityUserDto>(identityUser);

        return map;
    }

    public async Task<IdentityUserDto> GetAsync(string emil, string password)
    {
        var identityUser = await _identityUserRepository.GetAsync(emil, password);
        var map = _mapper.Map<IdentityUserDto>(identityUser);

        return map;
    }
}
