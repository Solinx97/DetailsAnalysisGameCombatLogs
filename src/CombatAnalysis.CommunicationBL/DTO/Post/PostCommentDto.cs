namespace CombatAnalysis.CommunicationBL.DTO.Post;

public class PostCommentDto
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public int PostId { get; set; }

    public string CustomerId { get; set; }
}
