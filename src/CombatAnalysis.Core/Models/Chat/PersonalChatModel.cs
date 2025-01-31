namespace CombatAnalysis.Core.Models.Chat;

public class PersonalChatModel
{
    public int Id { get; set; }

    public string Username { get; set; }

    public string InitiatorId { get; set; }

    public string CompanionId { get; set; }
}
