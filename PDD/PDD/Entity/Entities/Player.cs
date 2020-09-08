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
        public virtual bool Deadly => false;
        protected abstract PlayerIndex PlayerIndex { get; }

        protected Player()
        {
            CollidedWithEntity += OnCollidedWithEntity;
        }

        private void OnCollidedWithEntity(object? sender, IEntity e)
        {
            if(e.Deadliness.IsDeadly == DeadlinessInfo.Deadly.Deadly)
                Damage(e.Deadliness);
        }

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
                Damage(new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, ""));
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
                    var to = PortalData.Destinations[TileCheckArray.Lower.Index.Reverse() - Vector2.UnitX];
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
            }

            if (TileCheckArray.Lower.Tile.Deadliness.IsDeadlyBool) Damage(TileCheckArray.Lower.Tile.Deadliness);
            if (TileCheckArray.Upper.Tile.Deadliness.IsDeadlyBool) Damage(TileCheckArray.Upper.Tile.Deadliness);
            if (TileCheckArray.LowerLeft.Tile.Deadliness.IsDeadlyBool) Damage(TileCheckArray.LowerLeft.Tile.Deadliness);
            if (TileCheckArray.LowerRight.Tile.Deadliness.IsDeadlyBool) Damage(TileCheckArray.LowerRight.Tile.Deadliness);
            if (TileCheckArray.UpperLeft.Tile.Deadliness.IsDeadlyBool) Damage(TileCheckArray.UpperLeft.Tile.Deadliness);
            if (TileCheckArray.UpperRight.Tile.Deadliness.IsDeadlyBool) Damage(TileCheckArray.UpperRight.Tile.Deadliness);
            
            if (TileCheckArray.Lower.TileId == Tiles.Checkpoint) SetCheckPoint(TileCheckArray.Lower);
            if (TileCheckArray.Upper.TileId == Tiles.Checkpoint) SetCheckPoint(TileCheckArray.Upper);
            if (TileCheckArray.LowerLeft.TileId == Tiles.Checkpoint) SetCheckPoint(TileCheckArray.LowerLeft);
            if (TileCheckArray.LowerRight.TileId == Tiles.Checkpoint) SetCheckPoint(TileCheckArray.LowerRight);
            if (TileCheckArray.UpperLeft.TileId == Tiles.Checkpoint) SetCheckPoint(TileCheckArray.UpperLeft);
            if (TileCheckArray.UpperRight.TileId == Tiles.Checkpoint) SetCheckPoint(TileCheckArray.UpperRight);

            // Logging.Info($"{velocity}");
            base.SetNextPosition(environment, contentManager);
        }

        private void SetCheckPoint(PlacedTile tile)
        {
            StartingPosition = tile.Vector * 16;
            PddGame.CurrentIndicator = "Checkpoint reached!";
            PddGame.CurrentIndicatorTtl = 120;
            PddGame.CurrentIndicatorType = IndicatorType.Status;
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
            if (obj.GetType() != GetType()) return false;
            return Equals((Player) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(base.GetHashCode(), _texture, StartingPosition, (int) PlayerIndex);
        }

        private void Damage(DeadlinessInfo deadlinessInfo)
        {
            Position = StartingPosition;
            Velocity = Vector2.Zero;
            PddGame.CurrentIndicator = deadlinessInfo.Message;
            PddGame.CurrentIndicatorTtl = 120;
            PddGame.CurrentIndicatorType = IndicatorType.Status;
            Death?.Invoke(this, EventArgs.Empty);
        }

        public override Texture2D GetTexture2D() => _texture!;

        private Texture2D? _texture;
        public Vector2 StartingPosition = Vector2.One;
        public override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");

        public event EventHandler? Death;
    }
}