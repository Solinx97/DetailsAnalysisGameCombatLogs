using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels.CreateGroupChat
{
    public class CreateGroupChatPlayersViewModel : MvxViewModel
    {
        private readonly IHttpClientHelper _httpClientHelper;

        private IImprovedMvxViewModel _basicTemplate;
        private GroupChatModel _chatModel;

        public CreateGroupChatPlayersViewModel()
        {
            _httpClientHelper = Mvx.IoCProvider.GetSingleton<IHttpClientHelper>();

            CallbackCommand = new MvxCommand(Callback);
            CreateCommand = new MvxCommand(Create);
            CancelCommand = new MvxCommand(Cancel);
        }

        public CreateGroupChatPlayersViewModel(GroupChatModel chatModel) : this()
        {
            _chatModel = chatModel;
        }

        public IMvxCommand CallbackCommand { get; set; }

        public IMvxCommand CreateCommand { get; set; }

        public IMvxCommand CancelCommand { get; set; }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public void Callback()
        {
            WindowManager.CreateGroupChat.DataContext = new CreateGroupChatViewModel(_chatModel);
        }

        public void Create()
        {
            Task.Run(() => _httpClientHelper.PostAsync("GroupChat", JsonContent.Create(_chatModel)));
        }

        public void Cancel()
        {
            WindowManager.CreateGroupChat.Close();
        }
    }
}
