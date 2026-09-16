using CombatAnalysis.Core.Models.GameLogs;

namespace CombatAnalysis.Core.Interfaces.Entities;

public interface IDetailsEntity : IGeneralDetailsEntity
{
    UnitModel Unit { get; set; }

    UnitModel Target { get; set; }

    string UnitId { get; set; }
}
