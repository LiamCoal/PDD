namespace PDD.Tile
{
    public class NormalGroundDownFake : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundRotatedDown";
        internal override bool IsSolid => false;
        internal override bool CanFall => false; // just like minecraft
        internal override bool Deadly => false;
        internal override int TileId => 18;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}