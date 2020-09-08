using PDD.Tile;

namespace PDD.Entity
{
    public class TileCheckArray
    {
        public PlacedTile Lower, Upper, LowerLeft, UpperLeft, LowerRight, UpperRight;

        public TileCheckArray()
        {
            Lower = new PlacedTile();
            Upper = new PlacedTile();
            LowerLeft = new PlacedTile();
            LowerRight = new PlacedTile();
            UpperLeft = new PlacedTile();
            UpperRight = new PlacedTile();
        }
    }
}