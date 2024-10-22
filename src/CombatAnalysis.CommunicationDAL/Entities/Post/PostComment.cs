﻿namespace CombatAnalysis.CommunicationDAL.Entities.Post;

public class PostComment
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public int PostId { get; set; }

    public string AppUserId { get; set; }
}
