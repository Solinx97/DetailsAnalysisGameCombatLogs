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
        private GroupChatModel _groupChat;

        public CreateGroupChatViewModel()
        {
            _httpClientHelper = Mvx.IoCProvider.GetSingleton<IHttpClientHelper>();
            _memoryCache = Mvx.IoCProvider.GetSingleton<IMemoryCache>();

            AddUsersCommand = new MvxCommand(AddUsers);
            CreateCommand = new MvxAsyncCommand(CreateAsync);
            CancelCommand = new MvxCommand(Cancel);

            _groupChat = new GroupChatModel();
            GetPolicyTypeCommand = new MvxCommand<int>(GetChatType);
        }

        public CreateGroupChatViewModel(GroupChatModel chatModel) : this()
        {
            _groupChat = chatModel;
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

                WindowManager.CreateGroupChat.DataContext = new CreateGroupChatPlayersViewModel(_groupChat);
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
                    var groupChat = await response.Content.ReadFromJsonAsync<GroupChatModel>();
                    await CreateGroupChatUser(groupChat.Id, user.Id);

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
            _groupChat.Name = string.IsNullOrEmpty(Name) ? " " : Name;
            _groupChat.ShortName = " ";
            _groupChat.LastMessage = " ";
            _groupChat.ChatPolicyType = _policyType;
            _groupChat.MemberNumber = ChatConsts.ChatMemberNumber;
            _groupChat.OwnerId = user.Id;
        }

        private async Task<HttpResponseMessage> CreateGroupChat()
        {
            _httpClientHelper.BaseAddress = Port.ChatApi;

            var response = await _httpClientHelper.PostAsync("GroupChat", JsonContent.Create(_groupChat));
            return response;
        }

        private async Task CreateGroupChatUser(int groupChatId, string userId)
        {
            var groupChatUser = new GroupChatUserModel
            {
                GroupChatId = groupChatId,
                UserId = userId,
            };

            _httpClientHelper.BaseAddress = Port.ChatApi;

            await _httpClientHelper.PostAsync("GroupChatUser", JsonContent.Create(groupChatUser));
        }
    }
}
