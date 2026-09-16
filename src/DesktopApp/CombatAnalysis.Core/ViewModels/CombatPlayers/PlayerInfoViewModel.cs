using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.Models.GameLogs.CombatPlayerData;
using CombatAnalysis.Core.ViewModels.Base;

namespace CombatAnalysis.Core.ViewModels.CombatPlayers;

public class PlayerInfoViewModel(ICombatParserAPIService combatparserAPIService) : ParentTemplate<List<CombatPlayerModel>>
{
    private readonly ICombatParserAPIService _combatParserAPIService = combatparserAPIService;

    private List<CombatPlayerModel>? _players;
    private CombatPlayerModel? _selectedPlayer;
    private CombatPlayerStatsModel? _selectedPlayerStats;

    public List<CombatPlayerModel>? Players
    {
        get => _players;
        set
        {
            SetProperty(ref _players, value);

            if (value != null && value.Count > 0)
            {
                SelectedPlayer = value[0];
            }
        }
    }

    public CombatPlayerModel? SelectedPlayer
    {
        get => _selectedPlayer;
        set
        {
            SetProperty(ref _selectedPlayer, value);
            Task.Run(GetPlayerStatsAsync);
        }
    }

    public CombatPlayerStatsModel? SelectedPlayerStats
    {
        get => _selectedPlayerStats;
        set
        {
            SetProperty(ref _selectedPlayerStats, value);
        }
    }

    public override void Prepare(List<CombatPlayerModel> parameter)
    {
        Players = parameter;
    }

    public async Task GetPlayerStatsAsync()
    {
        if (SelectedPlayer != null)
        {
            var stats = await _combatParserAPIService.LoadPlayerStatsAsync($"CombatPlayer/getPlayerStats/{SelectedPlayer.Id}", CancellationToken.None);
            SelectedPlayerStats = stats;
        }
    }
}
