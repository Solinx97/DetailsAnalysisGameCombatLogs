using CombatAnalysis.CombatParserAPI.Models.WoWMidnight;
using CombatAnalysis.CombatParserAPI.Models.WoWMoPClassic;
using System.Text.Json.Serialization;

namespace CombatAnalysis.CombatParserAPI.Interfaces;

[JsonPolymorphic(TypeDiscriminatorPropertyName = "type")]
[JsonDerivedType(typeof(WoWMoPClassicPlayerStatsModel), "0")]
[JsonDerivedType(typeof(WoWMidnightPlayerStatsModel), "1")]
public interface IPlayerStatsModel
{
    int Strength { get; }

    int Agility { get; }

    int Intelligence { get; }

    int Stamina { get; }

    int Dodge { get; }

    int Parry { get; }

    int Block { get; }

    int Crit { get; }

    int Haste { get; }

    int Armor { get; }

    string Talents { get; }
}
