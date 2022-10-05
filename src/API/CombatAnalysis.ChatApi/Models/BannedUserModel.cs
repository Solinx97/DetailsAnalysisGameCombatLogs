namespace CombatAnalysis.ChatApi.Models
{
    public class BannedUserModel
    {
        public int Id { get; set; }

        public string WhomBannedId { get; set; }

        public string BannedUserId { get; set; }
    }
}
