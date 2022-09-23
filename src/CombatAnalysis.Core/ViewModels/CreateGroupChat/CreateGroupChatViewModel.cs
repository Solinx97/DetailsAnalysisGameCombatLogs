using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using CombatAnalysis.Core.Models.User;
using Microsoft.Extensions.Caching.Memory;
using MvvmCross;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace CombatAnalysis.Core.ViewModels.CreateGroupChat
{
    public class CreateGroupChatViewModel : MvxViewModel
    {
        private readonly IHttpClientHelper _httpClientHelper;
        private readonly IMemoryCache _memoryCache;

        private IImprovedMvxViewModel _basicTemplate;
        private string _name;
        private int _policyType;
        private GroupChatModel _chatModel;

        public CreateGroupChatViewModel()
        {
            _httpClientHelper = Mvx.IoCProvider.GetSingleton<IHttpClientHelper>();
            _memoryCache = Mvx.IoCProvider.GetSingleton<IMemoryCache>();

            AddUsersCommand = new MvxCommand(AddUsers);
            CreateCommand = new MvxAsyncCommand(CreateAsync);
            CancelCommand = new MvxCommand(Cancel);

            _chatModel = new GroupChatModel();
            GetPolicyTypeCommand = new MvxCommand<int>(GetChatType);
        }

        public CreateGroupChatViewModel(GroupChatModel chatModel) : this()
        {
            _chatModel = chatModel;
        }

        public IMvxCommand AddUsersCommand { get; set; }

        public IMvxAsyncCommand CreateCommand { get; set; }

        public IMvxCommand CancelCommand { get; set; }

        public IMvxCommand<int> GetPolicyTypeCommand { get; set; }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                SetProperty(ref _name, value);
            }
        }

        public void AddUsers()
        {
            var user = _memoryCache.Get<AppUserModel>("account");
            if (user != null)
            {
                UpdateGroupChatModel(user);

                WindowManager.CreateGroupChat.DataContext = new CreateGroupChatPlayersViewModel(_chatModel);
            }
        }

        public async Task CreateAsync()
        {
            var user = _memoryCache.Get<AppUserModel>("account");
            if (user != null)
            {
                UpdateGroupChatModel(user);

                var response = await CreateGroupChat();
                if (response.StatusCode == System.Net.HttpStatusCode.OK)
                {
                    CancelCommand.Execute();
                }
            }
        }

        public void Cancel()
        {
            WindowManager.CreateGroupChat.Close();
        }

        public void GetChatType(int policyType)
        {
            _policyType = policyType;
        }

        private void UpdateGroupChatModel(AppUserModel user)
        {
            _chatModel.Name = string.IsNullOrEmpty(Name) ? " " : Name;
            _chatModel.ShortName = " ";
            _chatModel.LastMessage = " ";
            _chatModel.ChatPolicyType = _policyType;
            _chatModel.MemberNumber = ChatConsts.ChatMemberNumber;
            _chatModel.OwnerId = user.Id;
        }

        private async Task<HttpResponseMessage> CreateGroupChat()
        {
            _httpClientHelper.BaseAddress = Port.ChatApi;

            var response = await _httpClientHelper.PostAsync("GroupChat", JsonContent.Create(_chatModel));
            return response;
        }
    }
}
