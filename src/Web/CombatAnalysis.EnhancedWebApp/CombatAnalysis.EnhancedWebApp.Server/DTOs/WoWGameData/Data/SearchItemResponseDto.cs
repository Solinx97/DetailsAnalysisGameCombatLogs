namespace CombatAnalysis.EnhancedWebApp.Server.DTOs.WoWGameData.Data;

public class SearchItemResponseDto
{
    public int Page { get; set; }

    public int PageSize { get; set; }

    public int MaxPageSize { get; set; }

    public int PageCount { get; set; }

    public SearchItemDto[] Results { get; set; }
}
