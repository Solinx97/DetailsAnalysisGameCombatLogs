using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.GameLogs;
using CombatAnalysis.Core.ViewModels.Base;
using CombatAnalysis.Core.ViewModels.CombatPlayers;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;

namespace CombatAnalysis.Core.ViewModels;

public class CombatPlayersViewModel : ParentTemplate<CombatModel>
{
    private readonly ICombatParserAPIService _combatParserAPIService;

    private int _selectedTabIndex = 1;

    public CombatPlayersViewModel(ICombatParserAPIService combatparserAPIService)
    {
        _combatParserAPIService = combatparserAPIService;

        Basic.Parent = this;
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 2);

        DamageDoneScoreVM = new DamageDoneScoreViewModel();
        DamageTakenScoreVM = new DamageTakenScoreViewModel();
        HealDoneScoreVM = new HealDoneScoreViewModel();
        ResourcesRecoveryScoreVM = new ResourcesRecoveryScoreViewModel();
        PlayerInfoVM = new PlayerInfoViewModel(combatparserAPIService);
    }

    public int SelectedTabIndex
    {
        get { return _selectedTabIndex; }
        set
        {
            SetProperty(ref _selectedTabIndex, value);
            if (value > 0)
            {
                OrderBy(value);
            }
        }
    }

    public CombatModel? Combat { get; set; }

    public DamageDoneScoreViewModel DamageDoneScoreVM { get; }

    public DamageTakenScoreViewModel DamageTakenScoreVM { get; }

    public HealDoneScoreViewModel HealDoneScoreVM { get; }

    public PlayerInfoViewModel PlayerInfoVM { get; }

    public ResourcesRecoveryScoreViewModel ResourcesRecoveryScoreVM { get; }

    public override void Prepare(CombatModel parameter)
    {
        Combat = parameter;
    }

    public override async Task Initialize()
    {
        if (Combat == null || Combat.CombatPlayers.Count != 0)
        {
            return;
        }

        var token = ((BasicTemplateViewModel)Basic).RequestCancelationToken();
        var combatPlayers = await _combatParserAPIService.LoadCombatPlayersAsync(Combat.Id, token);

        InitCombatPlayersData([.. combatPlayers]);

        await base.Initialize();
    }

    private void InitCombatPlayersData(List<CombatPlayerModel> combatPlayers)
    {
        if (Combat == null || combatPlayers.Count == 0)
        {
            return;
        }

        var updatedCombatPlayers = combatPlayers
            .Select(p => {
                var damageDonePercentages = (double)p.Unit.UnitInfo.DamageDone / (double)Combat.DamageDone;
                p.DamageDonePercentages = double.Round(damageDonePercentages * 100, 2);

                var healDonePercentages = (double)p.Unit.UnitInfo.HealDone / (double)Combat.HealDone;
                p.HealDonePercentages = double.Round(healDonePercentages * 100, 2);

                var damageTakenPercentages = (double)p.Unit.UnitInfo.DamageTaken / (double)Combat.DamageTaken;
                p.DamageTakenPercentages = double.Round(damageTakenPercentages * 100, 2);

                var resourcesRecoveryPercentages = (double)p.Unit.UnitInfo.ResourcesRecovery / (double)Combat.ResourcesRecovery;
                p.ResourcesRecoveryPercentages = double.Round(resourcesRecoveryPercentages * 100, 2);

                return p;
            })
            .OrderByDescending(p => p.Unit.UnitInfo.DamageDone)
            .ToList();

        GetCombatAverageInformation(Combat.Duration, updatedCombatPlayers);

        DamageDoneScoreVM.Prepare(updatedCombatPlayers);
        HealDoneScoreVM.Prepare(updatedCombatPlayers);
        DamageTakenScoreVM.Prepare(updatedCombatPlayers);
        ResourcesRecoveryScoreVM.Prepare(updatedCombatPlayers);
        PlayerInfoVM.Prepare(updatedCombatPlayers);

        base.Prepare();
    }

    private void OrderBy(int tabindex)
    {
        switch (tabindex)
        {
            case 1:
                DamageDoneScoreVM.OrderBy(tabindex);
                break;
            case 2:
                HealDoneScoreVM.OrderBy(tabindex);
                break;
            case 3:
                DamageTakenScoreVM.OrderBy(tabindex);
                break;
            case 4:
                ResourcesRecoveryScoreVM.OrderBy(tabindex);
                break;
            default:
                break;
        }
    }

    private static void GetCombatAverageInformation(string durationStr, List<CombatPlayerModel> players)
    {
        if (TimeSpan.TryParse(durationStr, out var duration))
        {
            foreach (var player in players)
            {
                player.DamageDonePerSecond = player.Unit.UnitInfo.DamageDone / duration.TotalSeconds;
                player.HealDonePerSecond = player.Unit.UnitInfo.HealDone / duration.TotalSeconds;
                player.ResourcesRecoveryPerSecond = player.Unit.UnitInfo.ResourcesRecovery / duration.TotalSeconds;
                player.DamageTakenPerSecond = player.Unit.UnitInfo.DamageTaken / duration.TotalSeconds;
            }
        }
    }
}