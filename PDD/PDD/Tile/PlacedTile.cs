using Microsoft.Xna.Framework;
using PDD.DataManagement;

namespace PDD.Tile
{
    public struct PlacedTile
    {
        public int X, Y;
        public int TileId;
        public Tile Tile => Tiles.TileList[TileId];
        public Vector2 Vector => new Vector2(X, Y);
        public LevelIndex Index => new LevelIndex(X, Y);
    }
}