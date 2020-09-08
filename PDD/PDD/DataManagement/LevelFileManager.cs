using System.IO;
using System.Text;
using System.Xml;

namespace PDD.DataManagement
{
    public static class LevelFileManager
    {
        public static Level Load(int levelNum)
        {
            using FileStream stream = File.OpenRead($"Levels/level:{levelNum}.xml");
            return Level.Load(stream);
        }
        
        public static void Save(int levelNum, Level level)
        {
            using StreamWriter stream = new StreamWriter(File.Create($"Levels/level:{levelNum}.xml"), Encoding.UTF8);
            stream.Write(level.ToString());
            stream.Close();
        }
    }
}