using CombatAnalysis.Core.Models.GameLogs;

namespace CombatAnalysis.Core.ViewModels.CombatPlayers;

public class DamageTakenScoreViewModel : BasicCombatPlayerViewModel
{
    public override void Prepare(List<CombatPlayerModel> parameter)
    {
        Players = parameter;
        _defaultPlayers = parameter;

        BestValue = Players.Max(p => p.Unit.UnitInfo.DamageTaken);

        var value = Players.Average(x => x.Unit.UnitInfo.DamageTaken);
        AverageValue = double.Round(value, 2);
        AverageVPS = Players.Average(x => x.DamageTakenPerSecond);

        TotalValue = Players.Sum(x => x.Unit.UnitInfo.DamageTaken);
        TotalVPS = Players.Sum(x => x.DamageTakenPerSecond);

        ValueType = 2;

        base.Prepare();
    }

    public override void OrderBy(int tabIndex)
    {
        if (Players == null)
        {
            return;
        }

        Players = [.. Players.OrderByDescending(p => p.Unit.UnitInfo.DamageTaken)];
    }
}
