﻿using CombatAnalysis.Identity.Interfaces;
using CombatAnalysis.IdentityDAL.Interfaces;

namespace CombatAnalysis.Identity.Services;

internal class AuthCodeService : IAuthCodeService
{
    private readonly IPkeRepository _authCodeRepository;

    public AuthCodeService(IPkeRepository authCodeRepository)
    {
        _authCodeRepository = authCodeRepository;
    }

    public void RemoveExpiredCodes()
    {
        _authCodeRepository.RemoveExpiredCodesAsync().Wait();
    }
}