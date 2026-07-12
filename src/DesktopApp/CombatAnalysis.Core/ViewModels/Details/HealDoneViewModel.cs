using CombatAnalysis.Core.Templates;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;

namespace CombatAnalysis.Core.ViewModels.Details;

public class HealDoneViewModel : BasicDetails
{
    public HealDoneViewModel()
    {
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 4);
    }
}
