namespace CombatAnalysis.ChatApi.Models;

public class PersonalChatModel
{
    public int Id { get; set; }

    public string InitiatorUsername { get; set; }

    public string CompanionUsername { get; set; }

    public string LastMessage { get; set; }

    public string InitiatorId { get; set; }

    public string CompanionId { get; set; }
}
