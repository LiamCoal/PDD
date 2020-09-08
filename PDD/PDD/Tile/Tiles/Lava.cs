namespace PDD.Tile
{
    public class Lava : Tile
    {
        protected override string TileTexturePath => "Tile/Lava";
        internal override bool IsSolid => false;
        internal override bool CanFall => false;
        internal override bool Deadly => true;
        internal override int TileId => 20;
    }
}