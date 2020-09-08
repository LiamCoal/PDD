namespace PDD.Tile
{
    public class NormalGroundRight : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundRotatedRight";
        internal override bool IsSolid => true;
        internal override bool CanFall => false; // just like minecraft
        internal override bool Deadly => false;
        internal override int TileId => 4;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}