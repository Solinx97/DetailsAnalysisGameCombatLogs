namespace CombatAnalysis.UploadingLogsApp.ViewModels;

public partial class CombatLogsViewModel : ViewModelBase
{
    public CombatLogsViewModel()
    {
    }

    public CombatLogsViewModel(ParsingCombatLogsViewModel parsingCombatLogs)
    {
        ParsingCombatLogs = parsingCombatLogs;
    }

    public ParsingCombatLogsViewModel ParsingCombatLogs { get; }
}
