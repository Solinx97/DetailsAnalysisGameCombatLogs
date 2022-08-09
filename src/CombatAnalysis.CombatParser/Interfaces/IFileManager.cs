using System.IO;

namespace CombatAnalysis.CombatParser.Interfaces
{
    public interface IFileManager
    {
        StreamReader StreamReader(string path);
    }
}
