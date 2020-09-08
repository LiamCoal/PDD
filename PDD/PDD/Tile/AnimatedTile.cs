using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PDD.DataManagement;

namespace PDD.Tile
{
    public abstract class AnimatedTile : Tile
    {
        private Texture2D[]? _textures;
        public abstract int FrameCount { get; }
        public abstract int FramesPerSecond { get; }
        public virtual bool RandomizeFrame => true;
        
        public override void LoadContent(ContentManager content)
        {
            List<Texture2D> texture2Ds = new List<Texture2D>();
            for (int i = 0; i < FrameCount; i++)
            {
                texture2Ds.Add(content.Load<Texture2D>($"{TileTexturePath}{i}"));
            }
            _textures = texture2Ds.ToArray();
        }

        public override Texture2D GetTexture2D(int x, int y, Level env)
        {
            var halfseconds = RandomizeFrame 
                ? DateTime.Now.Millisecond / (1000 / FramesPerSecond)
                : DateTime.Now.Millisecond / (1000 / FramesPerSecond) % FrameCount;
            var texindex = new Random(halfseconds | ((x % 256) << 16) | ((y % 256) << 24)).Next() % _textures!.Length;
            return _textures[RandomizeFrame ? texindex : halfseconds];
        }
    }
}