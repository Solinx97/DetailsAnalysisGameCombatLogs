using CombatAnalysis.Core.Models.GameLogs;

namespace CombatAnalysis.Core.ViewModels.CombatPlayers;

public class DamageDoneScoreViewModel : BasicCombatPlayerViewModel
{
    public override void Prepare(List<CombatPlayerModel> parameter)
    {
        Players = parameter;
        _defaultPlayers = parameter;

        BestValue = Players.Max(p => p.Unit.UnitInfo.DamageDone);

        var value = Players.Average(x => x.Unit.UnitInfo.DamageDone);
        AverageValue = double.Round(value, 2);
        AverageVPS = Players.Average(x => x.DamageDonePerSecond);

        TotalValue = Players.Sum(x => x.Unit.UnitInfo.DamageDone);
        TotalVPS = Players.Sum(x => x.DamageDonePerSecond);

        ValueType = 0;

        base.Prepare();
    }

    public override void OrderBy(int tabIndex)
    {
        if (Players == null)
        {
            return;
        }

        Players = [.. Players.OrderByDescending(p => p.Unit.UnitInfo.DamageDone)];
    }
}
