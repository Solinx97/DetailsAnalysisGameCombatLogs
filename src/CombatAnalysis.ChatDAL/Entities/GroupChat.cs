namespace CombatAnalysis.ChatDAL.Entities;

public class GroupChat
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string LastMessage { get; set; }

    public string CustomerId { get; set; }
}
