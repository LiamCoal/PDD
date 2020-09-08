namespace PDD.Tile
{
    public class NormalGroundLeftFake : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundRotatedLeft";
        internal override bool IsSolid => false;
        internal override bool CanFall => false; // just like minecraft
        internal override bool Deadly => false;
        internal override int TileId => 17;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}