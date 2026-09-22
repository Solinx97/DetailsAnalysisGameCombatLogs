using CombatAnalysis.WoW.CombatParser.Enums;

namespace CombatAnalysis.WoW.CombatParser.Entities;

public sealed record StatefulEvent(
    int Index,
    CombatEventType EventType,
    string Line);
