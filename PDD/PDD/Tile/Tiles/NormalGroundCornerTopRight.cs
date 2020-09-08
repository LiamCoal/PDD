namespace PDD.Tile
{
    public class NormalGroundCornerTopRight : Tile
    {
        protected override string TileTexturePath => "Tile/NormalGroundCornerTR";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override bool Deadly => false;
        internal override int TileId => 12;
    }
}