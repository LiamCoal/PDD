using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PDD.DataManagement;
using PDD.ResourceLoading;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

// ReSharper disable UnusedParameter.Global

namespace PDD.Entity
{
    
    // TODO make better than nothing
    public interface IEntity : IResourceLoader
    {
        public Vector2 Position { get; set; }
        public Size Size { get; }
        public Rectangle Rectangle => new Rectangle((int) Position.X, (int) Position.Y, Size.Width, Size.Height);
        public int Id { get; }
        public DeadlinessInfo Deadliness { get; }

        public Texture2D GetTexture2D();
        public void Update(GameTime gameTime, Level level);
        
        
    }
}