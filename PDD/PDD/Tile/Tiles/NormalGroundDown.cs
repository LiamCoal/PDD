namespace PDD.Tile
{
    public class NormalGroundDown : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundRotatedDown";
        internal override bool IsSolid => true;
        internal override bool CanFall => false; // just like minecraft
        internal override bool Deadly => false;
        internal override int TileId => 3;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}