using CombatAnalysis.Core.Templates;
using CombatAnalysis.Core.ViewModels.ViewModelTemplates;

namespace CombatAnalysis.Core.ViewModels.Details;

public class ResourceRecoveryViewModel : BasicDetails
{
    public ResourceRecoveryViewModel()
    {
        Basic.Handler.BasicPropertyUpdate(nameof(BasicTemplateViewModel.Step), 6);
    }
}
