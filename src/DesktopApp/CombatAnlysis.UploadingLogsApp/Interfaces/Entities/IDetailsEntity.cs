namespace CombatAnalysis.UploadingLogsApp.Interfaces.Entities;

public interface IDetailsEntity : IGeneralDetailsEntity
{
    string TargetGameId { get; }
}
