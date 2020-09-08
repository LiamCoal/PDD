namespace PDD.Tile
{
    public class Portal : Tile
    {
        protected override string TileTexturePath => "Tile/Portal";
        internal override bool IsSolid => false;
        internal override bool CanFall => false;
        internal override bool Deadly => false;
        internal override int TileId => -1;
    }
}