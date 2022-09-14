using CombatAnalysis.Core.Consts;
using CombatAnalysis.Core.Interfaces;
using CombatAnalysis.Core.Models.Chat;
using MvvmCross.Commands;
using MvvmCross.ViewModels;
using System;
using System.Collections.ObjectModel;

namespace CombatAnalysis.Core.ViewModels
{
    public class ChatViewModel : MvxViewModel
    {
        private IImprovedMvxViewModel _basicTemplate;
        private ObservableCollection<MessageModel> _messages;
        private bool _isMyMessage = true;
        private string _message;

        public ChatViewModel()
        {
            SendMessageCommand = new MvxCommand(Send);

            BasicTemplate = Templates.Basic;
            BasicTemplate.Parent = this;

            Messages = new ObservableCollection<MessageModel>
            {
                new MessageModel { Username = "Kiril", IsMyMessage = false, Message = "Hello, I'm Alex", Time = TimeSpan.Parse("10:11"), DayTimeType = Enums.WhenType.Yesterday }
            };
        } 

        public IMvxCommand SendMessageCommand { get; set; }

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

        public string Message
        {
            get { return _message; }
            set
            {
                SetProperty(ref _message, value);
            }
        }

        public void Send()
        {
            var newMessage = new MessageModel
            {
                IsMyMessage = true,
                DayTimeType = Enums.WhenType.Today,
                Message = Message,
                Time = TimeSpan.Parse($"{DateTimeOffset.UtcNow.Hour}:{DateTimeOffset.UtcNow.Minute}")
            };

            Messages.Add(newMessage);
            Message = string.Empty;
        }
    }
}
