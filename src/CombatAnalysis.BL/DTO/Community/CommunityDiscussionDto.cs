﻿namespace CombatAnalysis.BL.DTO.Community;

public class CommunityDiscussionDto
{
    public int Id { get; set; }

    public string Title { get; set; }

    public string Content { get; set; }

    public DateTimeOffset When { get; set; }

    public string CustomerId { get; set; }

    public int CommunityId { get; set; }
}