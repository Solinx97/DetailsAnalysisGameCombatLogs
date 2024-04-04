using CombatAnalysis.CombatParser.Core;
using CombatAnalysis.CombatParser.Entities;
using Microsoft.Extensions.Logging;

namespace CombatAnalysis.CombatParser.Patterns;

public class CombatDetailsClassInfo : CombatDetailsTemplate
{
    private readonly string[] _damageVariations = new string[]
    {
        CombatLogKeyWords.SpellDamage,
        CombatLogKeyWords.SwingDamage,
        CombatLogKeyWords.SpellPeriodicDamage,
        CombatLogKeyWords.SwingMissed,
        CombatLogKeyWords.DamageShieldMissed,
        CombatLogKeyWords.RangeDamage,
        CombatLogKeyWords.SpellMissed,
        CombatLogKeyWords.SpellSummon,
    };
    private readonly string[] _healVariations = new string[]
    {
        CombatLogKeyWords.SpellHeal,
        CombatLogKeyWords.SpellPeriodicHeal,
        CombatLogKeyWords.SpellAbsorbed,
    };
    private readonly string[] _absorbVariations = new string[]
    {
        CombatLogKeyWords.SpellAbsorbed,
    };
    private readonly ILogger _logger;

    private readonly Dictionary<string, string> _specs;
    private readonly Dictionary<string, string> _classes;

    public CombatDetailsClassInfo(ILogger logger, Dictionary<string, string> specs, Dictionary<string, string> classes) : base()
    {
        _logger = logger;
        _specs = specs;
        _classes = classes;
    }

    public override int GetData(string playerId, List<string> combatData)
    {
        try
        {
            if (string.IsNullOrEmpty(playerId))
            {
                throw new ArgumentNullException(playerId);
            }

            PlayerParseInfo = new PlayerParseInfo();

            var specId = GetSpecializationId(playerId, combatData);
            PlayerParseInfo.SpecId = specId;
            if (specId < 0)
            {
                return specId;
            }

            var classId = GetPlayerClassInfo(specId);
            PlayerParseInfo.ClassId = classId;

            return classId;
        }
        catch (ArgumentNullException ex)
        {
            _logger.LogError(ex, ex.Message, playerId);

            return -1;
        }
    }

    private int GetSpecializationId(string playerId, List<string> combatData)
    {
        var damageSpells = GetDamageSpells(playerId, combatData);
        foreach (var item in _specs)
        {
            var isUseThisSpec = damageSpells.Contains(item.Value);
            if (isUseThisSpec)
            {
                return int.Parse(item.Key);
            }
        }

        var healSpells = GetHealSpells(playerId, combatData);
        foreach (var item in _specs)
        {
            var isUseThisSpec = healSpells.Contains(item.Value);
            if (isUseThisSpec)
            {
                return int.Parse(item.Key);
            }
        }

        var absorbSpells = GetAbsorbSpells(playerId, combatData);
        foreach (var item in _specs)
        {
            var isUseThisSpec = absorbSpells.Contains(item.Value);
            if (isUseThisSpec)
            {
                return int.Parse(item.Key);
            }
        }

        return -1;
    }

    private int GetPlayerClassInfo(int specId)
    {
        var classId = _classes.FirstOrDefault(x => x.Value.Contains(specId.ToString())).Key;

        return int.Parse(classId);
    }

    private HashSet<string> GetDamageSpells(string playerId, List<string> combatData)
    {
        var spells = new HashSet<string>();
        foreach (var item in combatData)
        {
            var itemHasDamageVariation = _damageVariations.Any(item.Contains);
            var succesfullCombatDataInformation = GetUsefulInformation(item);

            if (!itemHasDamageVariation)
            {
                continue;
            }

            var spellOrItemName = GetDamageSpellOrItemName(playerId, succesfullCombatDataInformation);
            if (string.IsNullOrEmpty(spellOrItemName))
            {
                continue;
            }

            spells.Add(spellOrItemName);

            if (item.Contains(playerId))
            {
                spells.Add(spellOrItemName);
            }
        }

        return spells;
    }

    private HashSet<string> GetHealSpells(string playerId, List<string> combatData)
    {
        var spells = new HashSet<string>();
        foreach (var item in combatData)
        {
            var itemHasHealVariation = _healVariations.Any(item.Contains);
            var succesfullCombatDataInformation = GetUsefulInformation(item);

            if (!itemHasHealVariation)
            {
                continue;
            }

            var spellOrItemName = GetHealSpellOrItemName(playerId, succesfullCombatDataInformation);
            if (string.IsNullOrEmpty(spellOrItemName))
            {
                continue;
            }

            spells.Add(spellOrItemName);

            if (item.Contains(playerId))
            {
                spells.Add(spellOrItemName);
            }
        }

        return spells;
    }

    private HashSet<string> GetAbsorbSpells(string playerId, List<string> combatData)
    {
        var spells = new HashSet<string>();
        foreach (var item in combatData)
        {
            var itemHasHealVariation = _absorbVariations.Any(item.Contains);
            var succesfullCombatDataInformation = GetUsefulInformation(item);

            if (!itemHasHealVariation)
            {
                continue;
            }

            var spellOrItemName = GetAbsorbSpellOrItemName(playerId, succesfullCombatDataInformation);
            if (string.IsNullOrEmpty(spellOrItemName))
            {
                continue;
            }

            spells.Add(spellOrItemName);

            if (item.Contains(playerId))
            {
                spells.Add(spellOrItemName);
            }
        }

        return spells;
    }

    private static string GetDamageSpellOrItemName(string playerId, List<string> combatData)
    {
        if (string.Equals(combatData[1], CombatLogKeyWords.SpellSummon, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatData[1], CombatLogKeyWords.SwingDamage, StringComparison.OrdinalIgnoreCase))
        {
            return string.Empty;
        }

        if (!combatData[2].Contains(CombatLogKeyWords.Player))
        {
            return string.Empty;
        }
        else if (!combatData[2].Equals(playerId))
        {
            return string.Empty;
        }

        string spellOrItem;
        if (string.Equals(combatData[1], CombatLogKeyWords.SwingDamageLanded, StringComparison.OrdinalIgnoreCase)
            || string.Equals(combatData[1], CombatLogKeyWords.SwingMissed, StringComparison.OrdinalIgnoreCase))
        {
            spellOrItem = CombatLogKeyWords.MeleeDamage;
        }
        else
        {
            spellOrItem = combatData[11].Trim('"');
        }

        return spellOrItem;
    }

    private static string GetHealSpellOrItemName(string playerId, List<string> combatData)
    {
        if (!combatData[2].Equals(playerId))
        {
            return string.Empty;
        }

        var spellOrItem = combatData[11].Trim('"');

        return spellOrItem;
    }

    private static string GetAbsorbSpellOrItemName(string playerId, List<string> combatData)
    {
        if (!combatData[10].Equals(playerId) && !combatData[13].Equals(playerId))
        {
            return string.Empty;
        }

        var spellOrItem = combatData[^4].Trim('"');

        return spellOrItem;
    }
}
