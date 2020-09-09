using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using PDD.ConsoleOutput;
using PDD.DataManagement;
using PDD.Tile;

namespace PDD.Entity
{
    public class Cat : MovingEntity
    {
        public override void LoadContent(ContentManager content)
        {
            _leftTexture2D = content.Load<Texture2D>("Entity/CatLeft");
            _rightTexture2D = content.Load<Texture2D>("Entity/CatRight");
        }
        
        private enum Facing { Left, Right }
        public override Vector2 Position { get; set; }
        public override Size Size => new Size(16, 16);
        public override int Id => 2;
        public override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Deadly, "You were scratched...");
        private Texture2D _leftTexture2D, _rightTexture2D;
        private Facing _facing = Facing.Left;
        public override Texture2D GetTexture2D() => _facing == Facing.Left ? _leftTexture2D : _rightTexture2D;
        public override void SetNextPosition(Level environment, ContentManager contentManager)
        {
            base.SetNextPosition(environment, contentManager);
            if ((_facing == Facing.Left ? TileCheckArray!.LowerLeft : TileCheckArray!.LowerRight).Tile.IsSolid)
            {
                //Logging.Info($"{(_facing == Facing.Left ? TileCheckArray!.LowerLeft : TileCheckArray!.LowerRight).Vector} = {(_facing == Facing.Left ? TileCheckArray!.LowerLeft : TileCheckArray!.LowerRight).Tile}");
                _facing = _facing == Facing.Left ? Facing.Right : Facing.Left;
            }
            Velocity = new Vector2(_facing == Facing.Left ? -1.0f : 1.0f, Velocity.Y);
        }
    }
}