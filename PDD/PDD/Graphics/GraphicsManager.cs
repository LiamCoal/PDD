using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PDD.ResourceLoading;

namespace PDD.Graphics
{
    public class GraphicsManager : IResourceLoader
    {
        public class DrawableObject
        {
            public Texture2D Texture = null!;
            public Vector2 Position;
            // ReSharper disable once ConvertToConstant.Global
            // ReSharper disable once FieldCanBeMadeReadOnly.Global
            public float Scale = 1.0f;
        }

        /// <summary>
        /// The effects layer.
        /// Intended for effects.
        /// </summary>
        /// <remarks>Technically layer 3.</remarks>
        // ReSharper disable once CollectionNeverUpdated.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly List<DrawableObject> EffectLayer = new List<DrawableObject>(); // unused

        /// <summary>
        /// The main object layer.
        /// Intended to be used for rendering terrain.
        /// </summary>
        /// <remarks>Technically layer 2.</remarks>
        public static readonly List<DrawableObject> MainLayer = new List<DrawableObject>();

        /// <summary>
        /// Background layer.
        /// </summary>
        /// <remarks>Technically layer 1.</remarks>
        // ReSharper disable once CollectionNeverUpdated.Global
        // ReSharper disable once MemberCanBePrivate.Global
        public static readonly List<DrawableObject> BackgroundLayer = new List<DrawableObject>(); // unused

        private static Texture2D? MissingTexture;

        /// <summary>
        /// Allows the drawing of layers.
        /// </summary>
        /// <param name="spriteBatch"></param>
        internal static void Draw(ref SpriteBatch spriteBatch)
        {
            Draw0(ref spriteBatch, BackgroundLayer);
            Draw0(ref spriteBatch, MainLayer);
            Draw0(ref spriteBatch, EffectLayer);
        }

        private static void Draw0(ref SpriteBatch spriteBatch, IEnumerable<DrawableObject> drawables)
        {
            foreach (var drawable in drawables)
            {
                var obj = drawable;
                obj.Position.X /= 1;
                spriteBatch.Draw(obj.Texture ?? MissingTexture, obj.Position, null, Color.White, 0f, 
                    Vector2.Zero, obj.Scale, SpriteEffects.None, 0f); 
                //Logging.Info($"{obj.Position.X} {obj.Position.Y}: {obj.Texture}");
            }
        }

        public void LoadContent(ContentManager content) => MissingTexture = content.Load<Texture2D>("TextureMissing");
    }
}