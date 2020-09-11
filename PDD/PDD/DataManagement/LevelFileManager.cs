using System.IO;
using System.Text;
using System.Xml;

namespace PDD.DataManagement
{
    public static class LevelFileManager
    {
        public static Level Load(int levelNum) => Level.Load($"Levels/level:{levelNum}");

        public static void Save(int levelNum, Level level) => level.Save($"Levels/level:{levelNum}");
    }
}