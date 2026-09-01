using CombatAnalysis.WoW_5_5_4.CombatParser.Interfaces;

namespace CombatAnalysis.WoW_5_5_4.CombatParser.Core;

public class FileManager : IFileManager
{
    public StreamReader StreamReader(string path)
        => new(path);

    public Task<string[]> ReadAllLinesAsync(string path, CancellationToken cancellationToken)
        => File.ReadAllLinesAsync(path, cancellationToken);
}
