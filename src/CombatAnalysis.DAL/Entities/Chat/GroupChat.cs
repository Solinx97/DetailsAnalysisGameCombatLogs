namespace CombatAnalysis.DAL.Entities.Chat;

public class GroupChat
{
    public int Id { get; set; }

    public string Name { get; set; }

    public string ShortName { get; set; }

    public string LastMessage { get; set; }

    public string OwnerId { get; set; }
}
