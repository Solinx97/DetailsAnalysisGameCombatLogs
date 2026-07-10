using CombatAnalysis.Core.Templates;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;

namespace CombatAnalysis.Core.ViewModels.Details;

public class DamageDoneViewModel : BasicDetails
{
    public DamageDoneViewModel()
    {
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 3);
    }
}
