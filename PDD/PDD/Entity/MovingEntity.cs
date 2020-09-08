using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using PDD.DataManagement;
using PDD.Tile;
// ReSharper disable EventNeverInvoked.Global
// ReSharper disable EventNeverSubscribedTo.Global

namespace PDD.Entity
{
    public abstract class MovingEntity : IEntity
    {
        protected Vector2 Velocity { get; set; }
        private const float Gravity = 0.2f;
        public TileCheckArray? TileCheckArray;
        protected bool AllowGrounding = true;
        protected bool Grounded;

        public virtual void SetNextPosition(Level environment, ContentManager contentManager)
        {
            Velocity += new Vector2(-(Velocity.X / 100.0f), Gravity);
            
            TileCheckArray = new TileCheckArray();
            
            var tileLower = environment.GetTile(TileCheckArray.Lower.X = (int) Math.Round(Position.X / 16),
                TileCheckArray.Lower.Y =
                    (int) Math.Floor(Position.Y / 16 + Math.Ceiling(2.0)));
            var tileUpper = environment.GetTile(TileCheckArray.Upper.X = (int) Math.Round(Position.X / 16),
                TileCheckArray.Upper.Y =
                    (int) Math.Floor(Position.Y / 16 + Math.Ceiling(-1.0)));
            var tileLowerLeft = environment.GetTile(TileCheckArray.LowerLeft.X = (int) Math.Ceiling(Position.X / 16 - 1),
                TileCheckArray.LowerLeft.Y =
                    (int) Math.Floor(Position.Y / 16 + Math.Ceiling(1.0)));
            var tileUpperLeft = environment.GetTile(TileCheckArray.LowerRight.X = (int) Math.Ceiling(Position.X / 16 - 1),
                TileCheckArray.LowerRight.Y =
                    (int) Math.Floor(Position.Y / 16 + Math.Ceiling(0.0)));
            var tileLowerRight = environment.GetTile(TileCheckArray.UpperLeft.X = (int) Math.Floor(Position.X / 16 + 1),
                TileCheckArray.UpperLeft.Y =
                    (int) Math.Floor(Position.Y / 16 + Math.Ceiling(1.0)));
            var tileUpperRight = environment.GetTile(TileCheckArray.UpperRight.X = (int) Math.Floor(Position.X / 16 + 1),
                TileCheckArray.UpperRight.Y =
                    (int) Math.Floor(Position.Y / 16 + Math.Ceiling(0.0)));

            TileCheckArray.Lower.TileId = tileLower;
            TileCheckArray.Upper.TileId = tileUpper;
            TileCheckArray.LowerLeft.TileId = tileLower;
            TileCheckArray.LowerRight.TileId = tileLower;
            TileCheckArray.UpperLeft.TileId = tileUpper;
            TileCheckArray.UpperRight.TileId = tileUpper;

            // Logging.Info($"{environment._entities.IndexOf(this)} Checking tile at {Position.X / 16}, {Position.Y / 16 + GetTexture2D().Height}");
            Grounded = Tiles.TileList[tileLower].IsSolid && tileLower != Tiles.Air;
            var velocity = Velocity;
            if (Tiles.TileList[tileLower].IsSolid && tileLower != Tiles.Air && AllowGrounding)
            {
                // Logging.Info($"Found tile {environment.GetTile((int) (Position.X / 16), (int) (Position.Y / 16 + GetTexture2D().Height))}");
                
                velocity.Y = 0;
                Position = new Vector2(Position.X, (float) (Math.Floor(Position.Y / 16) * 16));
                TouchingTile?.Invoke(this, TileCheckArray.Lower);
            }

            if (Tiles.TileList[tileLowerRight].IsSolid && Velocity.X > 0)
            {
                velocity.X = 0;
                //Position = new Vector2((float) (Math.Floor(Position.X / 16) * 16), Position.Y);
                TouchingTile?.Invoke(this, TileCheckArray.LowerRight);
            }

            if (Tiles.TileList[tileUpperRight].IsSolid && Velocity.X > 0)
            {
                velocity.X = 0;
                //Position = new Vector2((float) (Math.Floor(Position.X / 16) * 16), Position.Y);
                TouchingTile?.Invoke(this, TileCheckArray.UpperRight);
            }
            
            if (Tiles.TileList[tileLowerLeft].IsSolid && Velocity.X < 0)
            {
                velocity.X = 0;
                //Position = new Vector2((float) (Math.Floor(Position.X / 16) * 16), Position.Y);
                TouchingTile?.Invoke(this, TileCheckArray.LowerLeft);
            }

            if (Tiles.TileList[tileUpperLeft].IsSolid && Velocity.X < 0)
            {
                velocity.X = 0;
                //Position = new Vector2((float) (Math.Floor(Position.X / 16) * 16), Position.Y);
                TouchingTile?.Invoke(this, TileCheckArray.UpperLeft);
            }

            if (Tiles.TileList[tileUpper].IsSolid)
            {
                if(tileUpper == Tiles.GrippyCeiling &&
                   !Keyboard.GetState().IsKeyDown(Keys.S)) velocity.Y = 0;
                Position = new Vector2(Position.X, (float) (Math.Floor(Position.Y / 16) * 16 +
                                                            (tileUpper == Tiles.GrippyCeiling &&
                                                             !Keyboard.GetState().IsKeyDown(Keys.S) ? 0 : 16)));
            }
            
            Velocity = velocity;
            ChangeVelocity();
            Position += Velocity;
            // Logging.Info(Position.ToString());
        }
        
        // ReSharper disable once VirtualMemberNeverOverridden.Global
        protected virtual void ChangeVelocity() {}
        public abstract void LoadContent(ContentManager content);
        public event EventHandler<PlacedTile>? TouchingTile;
        public abstract Vector2 Position { get; set; }
        public abstract int Id { get; }
        public abstract Texture2D GetTexture2D();
        public virtual void Update(GameTime gameTime, Level level) { }

        protected bool Equals(MovingEntity other)
        {
            return Equals(TileCheckArray, other.TileCheckArray) && AllowGrounding == other.AllowGrounding &&
                   Grounded == other.Grounded && Velocity.Equals(other.Velocity) && Position.Equals(other.Position) &&
                   Id == other.Id;
        }

        public override bool Equals(object? obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((MovingEntity) obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(TileCheckArray, AllowGrounding, Grounded, Velocity, Position, Id);
        }

        public override string ToString() =>
            $"{nameof(TileCheckArray)}: {TileCheckArray}, {nameof(AllowGrounding)}: {AllowGrounding}, {nameof(Grounded)}: {Grounded}, {nameof(Velocity)}: {Velocity}, {nameof(Position)}: {Position}, {nameof(Id)}: {Id}";
    }
    
    [Obsolete("Use MovingEntity instead.")]
    // ReSharper disable once UnusedType.Global
    // ReSharper disable once InconsistentNaming
    public abstract class IMovingEntity : MovingEntity {}
}