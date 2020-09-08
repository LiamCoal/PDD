namespace PDD.Tile
{
    public class NormalGroundRightFake : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundRotatedRight";
        internal override bool IsSolid => false;
        internal override bool CanFall => false; // just like minecraft
        internal override bool Deadly => false;
        internal override int TileId => 19;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}