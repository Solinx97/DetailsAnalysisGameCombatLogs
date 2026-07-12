using CombatAnalysis.Core.Templates;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;

namespace CombatAnalysis.Core.ViewModels.Details;

public class DamageTakenViewModel : BasicDetails
{
    public DamageTakenViewModel()
    {
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 5);
    }
}
