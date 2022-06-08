using CombatAnalysis.Core.Commands;
using CombatAnalysis.Core.Interfaces;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.ViewModels
{
    public class ResourceDetailsViewModel : MvxViewModel
    {
        private readonly IMvxNavigationService _mvvmNavigation;
        private readonly IViewModelConnect _handler;

        private MvxViewModel _basicTemplate;

        public ResourceDetailsViewModel(IMvxNavigationService mvvmNavigation)
        {
            _mvvmNavigation = mvvmNavigation;

            _handler = new ViewModelMConnect();
            BasicTemplate = new BasicTemplateViewModel(this, _handler, _mvvmNavigation);

            _handler.PropertyUpdate<BasicTemplateViewModel>(BasicTemplate, "Step", 6);
        }

        public MvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }
    }
}
