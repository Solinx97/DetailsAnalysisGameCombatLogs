namespace CombatAnalysis.BL.DTO.Chat;

public class PersonalChatDto
{
    public int Id { get; set; }

    public string LastMessage { get; set; }

    public string InitiatorId { get; set; }

    public string CompanionId { get; set; }
}
