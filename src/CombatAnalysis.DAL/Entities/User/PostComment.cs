﻿namespace CombatAnalysis.DAL.Entities.User;

public class PostComment
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public int PostId { get; set; }

    public string OwnerId { get; set; }
}