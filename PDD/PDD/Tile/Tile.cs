using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PDD.DataManagement;
using PDD.ResourceLoading;
using Logging = PDD.ConsoleOutput.Logging;

namespace PDD.Tile
{
    public abstract class Tile : IResourceLoader
    {
        private Texture2D _texture = null!;
        protected abstract string TileTexturePath { get; }
        internal abstract bool IsSolid { get; } // TODO unused
        // ReSharper disable once UnusedMember.Global
        internal abstract bool CanFall { get; } // TODO unused
        internal abstract DeadlinessInfo Deadliness { get; }
        internal abstract int TileId { get; }

        public virtual void LoadContent(ContentManager content)
        {
            Logging.Info($"Loading Tile: {TileTexturePath}");
            _texture = content.Load<Texture2D>(TileTexturePath);
            Logging.Info($"{_texture}");
        }

        // ReSharper disable once UnusedParameter.Global
        public virtual Texture2D GetTexture2D(int x, int y, Level env) => _texture;
    }
}