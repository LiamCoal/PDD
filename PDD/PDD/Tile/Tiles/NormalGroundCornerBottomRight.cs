namespace PDD.Tile
{
    public class NormalGroundCornerBottomRight : Tile
    {
        protected override string TileTexturePath => "Tile/NormalGroundCornerBR";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override bool Deadly => false;
        internal override int TileId => 9;
    }
}