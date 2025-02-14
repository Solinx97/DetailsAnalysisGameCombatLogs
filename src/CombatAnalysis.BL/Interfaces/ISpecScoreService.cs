﻿using CombatAnalysis.BL.DTO;

namespace CombatAnalysis.BL.Interfaces;

public interface ISpecScoreService
{
    Task<IEnumerable<SpecializationScoreDto>> GetBySpecIdAsync(int specId, int bossId, int difficult);
}