namespace CombatAnalysis.ChatApi.Models
{
    public class InviteToGroupChatModel
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public int Response { get; set; }

        public int GroupChatId { get; set; }
    }
}
