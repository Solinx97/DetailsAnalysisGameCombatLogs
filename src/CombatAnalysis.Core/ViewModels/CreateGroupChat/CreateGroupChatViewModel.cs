using CombatAnalysis.Core.Interfaces;
using MvvmCross.Commands;
using MvvmCross.ViewModels;

namespace CombatAnalysis.Core.ViewModels.CreateGroupChat
{
    public class CreateGroupChatViewModel : MvxViewModel, IImprovedMvxViewModel
    {
        private IImprovedMvxViewModel _basicTemplate;

        public CreateGroupChatViewModel()
        {
            BasicTemplate = this;

            CreateGroupChatCommand = new MvxCommand(Create);
        }

        public IMvxCommand CreateGroupChatCommand { get; set; }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public IViewModelConnect Handler { get; set; }

        public IMvxViewModel Parent { get; set; }

        public void Create()
        {

        }
    }
}
