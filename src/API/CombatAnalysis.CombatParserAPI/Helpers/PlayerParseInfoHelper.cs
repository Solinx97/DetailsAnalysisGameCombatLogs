﻿using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.BL.Interfaces.General;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParserAPI.Consts;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Helpers;

internal class PlayerParseInfoHelper : IPlayerParseInfoHelper
{
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public PlayerParseInfoHelper(IMapper mapper, IServiceScopeFactory serviceScopeFactory)
    {
        _mapper = mapper;
        _serviceScopeFactory = serviceScopeFactory;
    }

    public async Task UploadPlayerParseInfoAsync(Combat combat, CombatPlayerModel combatPlayer, List<DamageDoneGeneral> damageDoneGeneralList, List<HealDoneGeneral> healDoneGeneralList)
    {
        var playerParseInfo = new PlayerParseInfoModel
        {
            Difficult = combat.Difficulty
        };

        var specId = GetSpecializationId(damageDoneGeneralList, healDoneGeneralList);
        if (specId < 0)
        {
            return;
        }

        playerParseInfo.SpecId = specId;

        var classId = GetPlayerClassInfo(specId);
        if (classId < 0)
        {
            return;
        }

        playerParseInfo.ClassId = classId;

        var bosses = PlayerInfoConfiguration.Bosses;
        foreach (var item in bosses)
        {
            if (item.Value == combat.Name)
            {
                playerParseInfo.BossId = int.Parse(item.Key);
                break;
            }
        }

        var mapData = _mapper.Map<CombatPlayerDto>(combatPlayer);

        await UploadEfficiencySpecializationAsync(mapData, playerParseInfo);
        await UploadPlayerParseInfoAsync(playerParseInfo, mapData.Id);
    }

    private static int GetSpecializationId(List<DamageDoneGeneral> damageDoneGeneralList, List<HealDoneGeneral> healDoneGeneralList)
    {
        var damageSpells = damageDoneGeneralList.Select(damageDone => damageDone.Spell).ToList();
        var specs = PlayerInfoConfiguration.Specs;
        foreach (var item in specs)
        {
            var isUseThisSpec = damageSpells.Contains(item.Value);
            if (isUseThisSpec)
            {
                return int.Parse(item.Key);
            }
        }

        var healSpells = healDoneGeneralList.Select(healDone => healDone.Spell).ToList();
        foreach (var item in specs)
        {
            var isUseThisSpec = healSpells.Contains(item.Value);
            if (isUseThisSpec)
            {
                return int.Parse(item.Key);
            }
        }

        return -1;
    }

    private static int GetPlayerClassInfo(int specId)
    {
        var classes = PlayerInfoConfiguration.Classes;
        var classId = classes?.FirstOrDefault(playerClass => playerClass.Value.Contains(specId.ToString())).Key;
        if (string.IsNullOrEmpty(classId))
        {
            return -1;
        }

        return int.Parse(classId);
    }

    private async Task UploadEfficiencySpecializationAsync(CombatPlayerDto combatPlayer, PlayerParseInfoModel playerParseInfo)
    {
        double damageScore = 0;
        double healScore = 0;

        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<ISpecScoreService>();

        var score = await scopedService.GetBySpecIdAsync(playerParseInfo.SpecId, playerParseInfo.BossId, playerParseInfo.Difficult);

        var scopedMutationService = scope.ServiceProvider.GetRequiredService<IMutationService<SpecializationScoreDto>>();
        if (!score.Any())
        {
            playerParseInfo.DamageEfficiency = 100;
            playerParseInfo.HealEfficiency = 100;

            var newSpecScore = new SpecializationScoreDto
            {
                SpecId = playerParseInfo.SpecId,
                BossId = playerParseInfo.BossId,
                Difficult = playerParseInfo.Difficult,
                Damage = combatPlayer.DamageDone,
                Heal = combatPlayer.HealDone,
                Updated = DateTimeOffset.UtcNow,
            };

            await scopedMutationService.CreateAsync(newSpecScore);

            return;
        }


        var specScore = score.FirstOrDefault();
        if (combatPlayer.DamageDone != 0 && specScore.Damage != 0)
        {
            damageScore = (double)combatPlayer.DamageDone / (double)specScore.Damage;
        }

        if (damageScore > 1)
        {
            specScore.Damage = combatPlayer.DamageDone;

            await scopedMutationService.UpdateAsync(specScore);

            playerParseInfo.DamageEfficiency = 100;
        }
        else
        {
            playerParseInfo.DamageEfficiency = (int)(damageScore * 100);
        }

        if (combatPlayer.HealDone != 0 && specScore.Heal != 0)
        {
            healScore = (double)combatPlayer.HealDone / (double)specScore.Heal;
        }

        if (healScore > 1)
        {
            specScore.Heal = combatPlayer.HealDone;

            await scopedMutationService.UpdateAsync(specScore);

            playerParseInfo.HealEfficiency = 100;
        }
        else
        {
            playerParseInfo.HealEfficiency = (int)(healScore * 100);
        }
    }

    private async Task UploadPlayerParseInfoAsync(PlayerParseInfoModel playerParseInfo, int combatPlayerId)
    {
        var mapData = _mapper.Map<PlayerParseInfoDto>(playerParseInfo);
        mapData.CombatPlayerId = combatPlayerId;

        using var scope = _serviceScopeFactory.CreateScope();
        var scopedService = scope.ServiceProvider.GetRequiredService<IMutationService<PlayerParseInfoDto>>();

        await scopedService.CreateAsync(mapData);
    }
}
