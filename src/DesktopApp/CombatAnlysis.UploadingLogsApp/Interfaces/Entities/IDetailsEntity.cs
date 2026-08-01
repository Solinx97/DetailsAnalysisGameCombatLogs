namespace CombatAnalysis.UploadingLogsApp.Interfaces.Entities;

public interface IDetailsEntity : IGeneralDetailsEntity
{
    string Creator { get; set; }

    string Target { get; set; }
}
