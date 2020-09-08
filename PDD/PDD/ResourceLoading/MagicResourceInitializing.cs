using System;
using System.Reflection;
using Microsoft.Xna.Framework.Content;
using PDD.ConsoleOutput;
using PDD.Tile;

namespace PDD.ResourceLoading
{
    internal static class MagicResourceInitializing
    {
        internal static void InitializeAll(ContentManager contentManager)
        {
            var asm = Assembly.GetExecutingAssembly();
            foreach (var type in asm.GetTypes())
            {
                if(type.IsAbstract || !typeof(IResourceLoader).IsAssignableFrom(type))
                    continue;
                if(!typeof(Tile.Tile).IsAssignableFrom(type))
                {
                    Logging.Info($"Loading content: {type}");
                    if (!(Activator.CreateInstance(type) is IResourceLoader obj))
                        throw new NullReferenceException($"{nameof(obj)} is null.");
                    obj.LoadContent(contentManager);
                }
                else
                {
                    Logging.Info($"Loading tile content: {type}");
                    if (!(Activator.CreateInstance(type) is Tile.Tile obj))
                        throw new NullReferenceException($"{nameof(obj)} is null.");
                    var tile = Tiles.TileList[obj.TileId];
                    tile.LoadContent(contentManager);
                    Tiles.TileList[obj.TileId] = tile;
                }
            }
        }
    }
}