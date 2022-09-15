namespace CombatAnalysis.DAL.Entities.Chat
{
    public class GroupChat
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int MemberNumber { get; set; }

        public int ChatPolicyType { get; set; }

        public string OwnerId { get; set; }
    }
}
