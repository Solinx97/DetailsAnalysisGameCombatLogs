using AutoMapper;
using CombatAnalysis.BL.DTO;
using CombatAnalysis.BL.Interfaces;
using CombatAnalysis.CombatParser.Entities;
using CombatAnalysis.CombatParserAPI.Consts;
using CombatAnalysis.CombatParserAPI.Interfaces;
using CombatAnalysis.CombatParserAPI.Models;

namespace CombatAnalysis.CombatParserAPI.Helpers;

internal class PlayerParseInfoHelper : IPlayerParseInfoHelper
{
    private readonly IMapper _mapper;
    private readonly IService<PlayerParseInfoDto, int> _playerParseService;
    private readonly ISpecScoreService<SpecializationScoreDto, int> _specializationScoreService;

    public PlayerParseInfoHelper(IMapper mapper, IService<PlayerParseInfoDto, int> playerParseService, 
        ISpecScoreService<SpecializationScoreDto, int> specializationScoreService)
    {
        _mapper = mapper;
        _playerParseService = playerParseService;
        _specializationScoreService = specializationScoreService;
    }

    public async Task UploadPlayerParseInfoAsync(CombatDto combat, CombatPlayerDto combatPlayer, List<DamageDoneGeneral> damageDoneGeneralList, List<HealDoneGeneral> healDoneGeneralList)
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

        await UploadEfficiencySpecializationAsync(combatPlayer, playerParseInfo);

        await UploadPlayerParseInfoAsync(playerParseInfo, combatPlayer.Id);
    }

    private static int GetSpecializationId(List<DamageDoneGeneral> damageDoneGeneralList, List<HealDoneGeneral> healDoneGeneralList)
    {
        var damageSpells = damageDoneGeneralList.Select(damageDone => damageDone.SpellOrItem).ToList();
        var specs = PlayerInfoConfiguration.Specs;
        foreach (var item in specs)
        {
            var isUseThisSpec = damageSpells.Contains(item.Value);
            if (isUseThisSpec)
            {
                return int.Parse(item.Key);
            }
        }

        var healSpells = healDoneGeneralList.Select(healDone => healDone.SpellOrItem).ToList();
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
        var classId = classes.FirstOrDefault(playerClass => playerClass.Value.Contains(specId.ToString())).Key;

        return int.Parse(classId);
    }

    private async Task UploadEfficiencySpecializationAsync(CombatPlayerDto combatPlayer, PlayerParseInfoModel playerParseInfo)
    {
        var score = await _specializationScoreService.GetBySpecIdAsync(playerParseInfo.SpecId, playerParseInfo.BossId, playerParseInfo.Difficult);

        if (!score.Any())
        {
            var newSpecScore = new SpecializationScoreDto
            {
                SpecId = playerParseInfo.SpecId,
                BossId = playerParseInfo.BossId,
                Difficult = playerParseInfo.Difficult,
                Damage = combatPlayer.DamageDone,
                Heal = combatPlayer.HealDone,
                Updated = DateTimeOffset.UtcNow,
            };

            await _specializationScoreService.CreateAsync(newSpecScore);

            playerParseInfo.DamageEfficiency = 100;
            playerParseInfo.HealEfficiency = 100;

            return;
        }

        var specScore = score.FirstOrDefault();
        double damageScore = combatPlayer.DamageDone / specScore.Damage;
        if (damageScore > 1)
        {
            specScore.Damage = combatPlayer.DamageDone;

            await _specializationScoreService.UpdateAsync(specScore);

            playerParseInfo.DamageEfficiency = 100;
        }
        else
        {
            playerParseInfo.DamageEfficiency = (int)(damageScore * 100);
        }

        double healScore = combatPlayer.HealDone / specScore.Heal;
        if (healScore > 1)
        {
            specScore.Heal = combatPlayer.HealDone;

            await _specializationScoreService.UpdateAsync(specScore);

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

        await _playerParseService.CreateAsync(mapData);
    }
}
