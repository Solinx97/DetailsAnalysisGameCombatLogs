namespace CombatAnalysis.DAL.Entities.Chat
{
    public class BannedUser
    {
        public int Id { get; set; }

        public string WhomBannedId { get; set; }

        public string BannedUserId { get; set; }
    }
}
