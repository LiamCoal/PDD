using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using PDD.DataManagement;
using PDD.ResourceLoading;
// ReSharper disable UnusedParameter.Global

namespace PDD.Entity
{
    
    // TODO make better than nothing
    public interface IEntity : IResourceLoader
    {
        public Vector2 Position { get; set; }
        public int Id { get; }

        public Texture2D GetTexture2D();
        public void Update(GameTime gameTime, Level level);
        
        
    }
}