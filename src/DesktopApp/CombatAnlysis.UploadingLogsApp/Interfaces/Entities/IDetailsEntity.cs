namespace CombatAnalysis.UploadingLogsApp.Interfaces.Entities;

public interface IDetailsEntity : IGeneralDetailsEntity
{
    string CreatorGameId { get; }

    string TargetGameId { get; }
}
