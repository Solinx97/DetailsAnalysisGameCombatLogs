namespace CombatAnalysis.CommunicationBL.DTO.Chat;

public class UnreadGroupChatMessageDto
{
    public int Id { get; set; }

    public string GroupChatUserId { get; set; }

    public int GroupChatMessageId { get; set; }
}
