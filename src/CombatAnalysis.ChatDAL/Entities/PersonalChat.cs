namespace CombatAnalysis.ChatDAL.Entities;

public class PersonalChat
{
    public int Id { get; set; }

    public string LastMessage { get; set; }

    public string InitiatorId { get; set; }

    public string CompanionId { get; set; }
}
