namespace CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces;

public interface IFileManager
{
    StreamReader StreamReader(string path);

    Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken);
}
