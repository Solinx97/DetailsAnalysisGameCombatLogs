using Microsoft.Win32;

namespace CombatAnalysis.WinCore
{
    public static class WinHandler
    {
        public static string FileOpen()
        {
            var file = new OpenFileDialog();
            file.ShowDialog();

            return file.FileName;
        }
    }
}