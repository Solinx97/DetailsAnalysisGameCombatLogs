﻿namespace CombatAnalysis.BL.DTO.Post;

public class PostCommentDto
{
    public int Id { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public int PostId { get; set; }

    public string OwnerId { get; set; }
}