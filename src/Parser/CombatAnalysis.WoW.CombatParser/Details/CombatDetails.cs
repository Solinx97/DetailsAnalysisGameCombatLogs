using CombatAnalysis.WoW.CombatParser.Entities;
using CombatAnalysis.WoW.CombatParser.Enums;
using CombatAnalysis.WoW.CombatParser.Interfaces;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace CombatAnalysis.WoW.CombatParser.Details;

public abstract class CombatDetails(ICombatParserHelper combatParserHelper, ILogger logger, ConcurrentDictionary<string, Unit> units)
{
    protected readonly ICombatParserHelper _combatParserHelper = combatParserHelper;

    public ILogger Logger { get; private set; } = logger;

    public ConcurrentDictionary<string, Unit> Units { get; protected set; } = units;

    public void Calculate(string[] combatData, DateTimeOffset combatStarted, DateTimeOffset combatFinished)
    {
        try
        {
            ArgumentNullException.ThrowIfNull(combatData, nameof(combatData));
            ArgumentOutOfRangeException.ThrowIfZero(combatData.Length);

            var combatDetailsManager = CreateCombatDetailsManager(combatStarted, combatFinished);
            var statefulEvents = new ConcurrentBag<StatefulEvent>();

            Parallel.For(
                0,
                combatData.Length,
                i =>
                {
                    var line = combatData[i];

                    var eventType = GetEventType(line);

                    if (eventType == CombatEventType.None)
                        return;

                    if (eventType == CombatEventType.Cast 
                        || eventType == CombatEventType.CastSuccess
                        || eventType == CombatEventType.Aura)
                    {
                        statefulEvents.Add(new StatefulEvent(i, eventType, line));

                        return;
                    }

                    var splitData = _combatParserHelper.SplitCombatData(line);

                    ParseStateless(splitData, eventType, combatDetailsManager);
                });

            foreach (var statefulEvent in statefulEvents.OrderBy(x => x.Index))
            {
                var splitData = _combatParserHelper.SplitCombatData(statefulEvent.Line);

                ParseStateful(splitData, statefulEvent.EventType,combatDetailsManager);
            }
        }
        catch (ArgumentNullException ex)
        {
            Logger.LogError("Some argument was null: {Param}", ex.ParamName);
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Logger.LogError("Some argument out of valid range: {Param}", ex.ParamName);
        }
    }

    protected abstract CombatDetailsManager CreateCombatDetailsManager(DateTimeOffset combatStarted, DateTimeOffset combatFinished);

    private void ParseStateless(string[] splitCombatData, CombatEventType eventType, CombatDetailsManager combatDetailsManager)
    {
        if (eventType == CombatEventType.DamageSuccess)
        {
            combatDetailsManager.GetHealth(splitCombatData, Units, UnitHealthStatus.Decrease);
        }
        else if (eventType == CombatEventType.Heal)
        {
            combatDetailsManager.GetHealth(splitCombatData, Units, UnitHealthStatus.Increase);
        }

        switch (eventType)
        {
            case CombatEventType.Damage:
                combatDetailsManager.GetDamageDone(splitCombatData, Units);
                break;
            case CombatEventType.Heal:
                combatDetailsManager.GetHealDone(splitCombatData, Units);
                break;
            case CombatEventType.Resources:
                combatDetailsManager.GetResourceRecovery(splitCombatData, Units);
                break;
            case CombatEventType.Absorb:
                combatDetailsManager.GetAbsorb(splitCombatData, Units);
                break;
            case CombatEventType.Death:
                combatDetailsManager.AddUnitDeath(splitCombatData, Units);
                break;
        }
    }

    private void ParseStateful(string[] splitCombatData, CombatEventType eventType, CombatDetailsManager combatDetailsManager)
    {
        if (eventType == CombatEventType.CastSuccess)
        {
            combatDetailsManager.GetPosition(splitCombatData, Units);
        }

        if (eventType == CombatEventType.Cast || eventType == CombatEventType.CastSuccess)
        {
            combatDetailsManager.GetCasts(splitCombatData, Units);
        }
        else if (eventType == CombatEventType.Aura)
        {
            combatDetailsManager.GetAuras(splitCombatData, units);
        }
    }

    private static CombatEventType GetEventType(string line)
    {
        var separatorIndex = line.IndexOf("  ", StringComparison.Ordinal);

        if (separatorIndex < 0)
            return CombatEventType.None;

        var eventStart = separatorIndex + 2;

        var eventEnd = line.IndexOf(',', eventStart);

        if (eventEnd < 0)
            eventEnd = line.Length;

        var eventName = line.AsSpan(eventStart, eventEnd - eventStart);

        return GetEventType(eventName);
    }

    private static CombatEventType GetEventType(ReadOnlySpan<char> eventName)
    {
        if (eventName.SequenceEqual("SPELL_DAMAGE")
            || eventName.SequenceEqual("SPELL_PERIODIC_DAMAGE")
            || eventName.SequenceEqual("RANGE_DAMAGE")
            || eventName.SequenceEqual("SWING_DAMAGE")
            || eventName.SequenceEqual("SWING_MISSED")
            || eventName.SequenceEqual("DAMAGE_SHIELD_MISSED")
            || eventName.SequenceEqual("SPELL_MISSED"))
            return CombatEventType.Damage;

        if (eventName.SequenceEqual("SPELL_DAMAGE")
            || eventName.SequenceEqual("SPELL_PERIODIC_DAMAGE")
            || eventName.SequenceEqual("RANGE_DAMAGE")
            || eventName.SequenceEqual("SWING_DAMAGE_LANDED"))
            return CombatEventType.DamageSuccess;

        if (eventName.SequenceEqual("SPELL_HEAL")
            || eventName.SequenceEqual("SPELL_PERIODIC_HEAL"))
            return CombatEventType.Heal;

        if (eventName.SequenceEqual("SPELL_ABSORBED"))
            return CombatEventType.Absorb;

        if (eventName.SequenceEqual("SPELL_ENERGIZE")
            || eventName.SequenceEqual("SPELL_PERIODIC_ENERGIZE"))
            return CombatEventType.Resources;

        if (eventName.SequenceEqual("SPELL_CAST_START")
            || eventName.SequenceEqual("SPELL_CAST_FAILED"))
            return CombatEventType.Cast;

        if (eventName.SequenceEqual("SPELL_CAST_SUCCESS"))
            return CombatEventType.CastSuccess;

        if (eventName.SequenceEqual("SPELL_AURA_APPLIED")
            || eventName.SequenceEqual("SPELL_AURA_REMOVED")
            || eventName.SequenceEqual("SPELL_AURA_APPLIED_DOSE")
            || eventName.SequenceEqual("SPELL_AURA_REMOVED_DOSE"))
            return CombatEventType.Aura;

        if (eventName.SequenceEqual("UNIT_DIED"))
            return CombatEventType.Death;

        return CombatEventType.None;
    }
}
