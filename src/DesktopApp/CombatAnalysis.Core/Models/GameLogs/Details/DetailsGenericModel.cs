namespace CombatAnalysis.Core.Models.GameLogs.Details;

public class DetailsGenericModel
{
    public string GenericModelType { get; set; } = string.Empty;

    public string GenericAPIName { get; set; } = string.Empty;

    public string ModelType { get; set; } = string.Empty;

    public string APIName { get; set; } = string.Empty;

    public CombatPlayerModel CombatPlayer { get; set; } = new();
}
