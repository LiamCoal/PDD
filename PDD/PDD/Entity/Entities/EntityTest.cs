using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PDD.DataManagement;
using PDD.Tile;

namespace PDD.Entity
{
    public class EntityTest : MovingEntity
    {

        private Texture2D _texture = null!;

        private static void EOnCollision(object? sender, IEntity e)
        {
            throw new NotImplementedException();
        }
        
        private static void EOnTileCollision(object? sender, PlacedTile e)
        {
            throw new NotImplementedException();
        }
        
        private static void EOnDamageTaken(object? sender, int e)
        {
            throw new NotImplementedException();
        }

        public override Vector2 Position { get; set; }

        public override Size Size => new Size(16, 32);
        public override int Id => Entities.Test;
        public override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Deadly, "You were tested...");
        public override Texture2D GetTexture2D() => _texture;

        public override void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>("Entity/Test");
        }
    }
}