using MvvmCross.ViewModels;
using System.ComponentModel;

namespace CombatAnalysis.Core.Models.Chat;

public class PersonalChatMessageModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string Message { get; set; }

    public TimeSpan Time { get; set; }

    public int PersonalChatId { get; set; }
}
