using System;
using System.IO;
using PDD.DataManagement;

namespace PDD.Start
{
    public static class Program
    {
        [STAThread]
        private static void Main(string[] args)
        {
            using var game = new PddGame();

            if (args.Length > 0)
            {
                PddGame.Mode = args[0] switch
                {
                    "editor" => Mode.LevelEditor,
                    _ => Mode.Default
                };
            }

            if (PddGame.Mode == Mode.LevelEditor && args.Length >= 2)
            {
                game.LevelLoad = args[1];
            }

            game.Run();
        }
    }
}