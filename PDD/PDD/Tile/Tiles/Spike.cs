namespace PDD.Tile
{
    public class Spike : Tile
    {
        protected override string TileTexturePath => "Tile/Spike";
        internal override bool IsSolid => true;
        internal override bool CanFall => true;
        internal override bool Deadly => true;
        internal override int TileId => 13;
    }
}