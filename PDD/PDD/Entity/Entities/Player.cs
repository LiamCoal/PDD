using System;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PDD.ConsoleOutput;
using PDD.DataManagement;
using PDD.Graphics;
using PDD.Start;
using PDD.Tile;

namespace PDD.Entity
{
    public abstract class Player : MovingEntity
    {
        public override Size Size => new Size(16, 32);
        public override bool Deadly => false;
        protected abstract PlayerIndex PlayerIndex { get; }

        public override void LoadContent(ContentManager content)
        {
            _texture = content.Load<Texture2D>($"Entity/Player{ToInt(PlayerIndex) - 1}");
        }

        private static int ToInt(PlayerIndex index)
        {
            return index switch
            {
                PlayerIndex.One => 1,
                PlayerIndex.Two => 2,
                PlayerIndex.Three => 3,
                PlayerIndex.Four => 4,
                _ => -1
            };
        }

        public override void SetNextPosition(Level environment, ContentManager contentManager)
        {
            var velocity = Velocity;
            // Logging.Info(velocity.ToString());
            AllowGrounding = true;
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                velocity.X += 0.05f;
                // Logging.Info("moving right");
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                velocity.X -= 0.05f;
                // Logging.Info("moving left");
            }

            if (Keyboard.GetState().IsKeyDown(Keys.R) && PddGame.Mode == Mode.Default)
            {
                Damage();
            }

            if (Keyboard.GetState().IsKeyDown(Keys.W) && Grounded)
            {
                velocity.Y -= 3.00f;
                AllowGrounding = false;
                // Logging.Info("Jumping");
            }

            Velocity = velocity;
            
            if(TileCheckArray == null)
            {
                base.SetNextPosition(environment, contentManager);
                return;
            }

            try
            {
                if (TileCheckArray.LowerLeft.TileId == Tiles.Portal || 
                    TileCheckArray.LowerRight.TileId == Tiles.Portal)
                {
                    var to = PortalData.Destinations[TileCheckArray.Lower.Index.Reverse()];
                    PddGame.FutureLevel = LevelFileManager.Load(to);
                    PddGame.FutureLevel.LoadEntityContent(contentManager);
                    PddGame.CurrentIndicator = "Teleporting...";
                    PddGame.CurrentIndicatorTtl = 120;
                    PddGame.CurrentIndicatorType = IndicatorType.Status;
                    return;
                }
            }
            catch (KeyNotFoundException e)
            {
                Logging.Error(e.Message);
                Damage();
            }

            if (TileCheckArray.Lower.Tile.Deadly) Damage();
            if (TileCheckArray.Upper.Tile.Deadly) Damage();
            if (TileCheckArray.LowerLeft.Tile.Deadly) Damage();
            if (TileCheckArray.LowerRight.Tile.Deadly) Damage();
            if (TileCheckArray.UpperLeft.Tile.Deadly) Damage();
            if (TileCheckArray.UpperRight.Tile.Deadly) Damage();
            
            if (TileCheckArray.Lower.TileId == Tiles.Checkpoint) StartingPosition = TileCheckArray.Lower.Vector * 16;
            if (TileCheckArray.Upper.TileId == Tiles.Checkpoint) StartingPosition = TileCheckArray.Upper.Vector * 16;
            if (TileCheckArray.LowerLeft.TileId == Tiles.Checkpoint) StartingPosition = TileCheckArray.LowerLeft.Vector * 16;
            if (TileCheckArray.LowerRight.TileId == Tiles.Checkpoint) StartingPosition = TileCheckArray.LowerRight.Vector * 16;
            if (TileCheckArray.UpperLeft.TileId == Tiles.Checkpoint) StartingPosition = TileCheckArray.UpperLeft.Vector * 16;
            if (TileCheckArray.UpperRight.TileId == Tiles.Checkpoint) StartingPosition = TileCheckArray.UpperRight.Vector * 16;

            // Logging.Info($"{velocity}");
            base.SetNextPosition(environment, contentManager);
        }

        protected bool Equals(Player other)
        {
            return base.Equals(other) && Equals(_texture, other._texture) && StartingPosition.Equals(other.StartingPosition) && PlayerIndex == other.PlayerIndex;
        }

        public override string ToString()
        {
            return $"{base.ToString()}, {nameof(_texture)}: {_texture}, {nameof(StartingPosition)}: {StartingPosition}, {nameof(PlayerIndex)}: {PlayerIndex}";
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((Player) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), _texture, StartingPosition, (int) PlayerIndex);
        }

        private void Damage()
        {
            Position = StartingPosition;
            Velocity = Vector2.Zero;
            PddGame.CurrentIndicator = "You died!";
            PddGame.CurrentIndicatorTtl = 120;
            PddGame.CurrentIndicatorType = IndicatorType.Status;
            Death?.Invoke(this, EventArgs.Empty);
        }

        public override Texture2D GetTexture2D() => _texture!;

        private Texture2D? _texture;
        public Vector2 StartingPosition = Vector2.One;
        
        public event EventHandler? Death;
    }
}