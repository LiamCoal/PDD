using System;
using Microsoft.Xna.Framework;
using PDD.Tile;

namespace PDD.Entity
{
    public class PlayerOne : Player

    {
        public override Vector2 Position { get; set; }
        public override int Id => Entities.PlayerOne;
        protected override PlayerIndex PlayerIndex => PlayerIndex.One;
    }
}