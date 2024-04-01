namespace CombatAnalysis.CommunicationDAL.Entities.Chat;

public class GroupChat
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string LastMessage { get; set; }

    public string CustomerId { get; set; }
}
