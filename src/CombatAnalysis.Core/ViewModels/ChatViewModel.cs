using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using MvvmCross.ViewModels;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel
    {
        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<MessageModel> _messages;
        private bool _isMyMessage = true;

        public ChatViewModel()
        {
            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;

            Messages = new ObservableCollection<MessageModel>
            {
                new MessageModel { Username = "Kiril", IsMyMessage = false, Message = "Hello, I'm Alex", MessageContentType = 0, Time = System.TimeSpan.Parse("10:11"), DayTimeType = Enums.WhenType.Yesterday }
            };
        }

        public IImprovedMvxViewModel BasicTemplate
        {
            get { return _basicTemplate; }
            set
            {
                SetProperty(ref _basicTemplate, value);
            }
        }

        public ObservableCollection<MessageModel> Messages
        {
            get { return _messages; }
            set
            {
                SetProperty(ref _messages, value);
            }
        }

        public bool IsMyMessage
        {
            get { return _isMyMessage; }
            set
            {
                SetProperty(ref _isMyMessage, value);
            }
        }
    }
}
