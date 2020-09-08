namespace PDD.Tile
{
    public class NormalGroundLeft : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundRotatedLeft";
        internal override bool IsSolid => true;
        internal override bool CanFall => false; // just like minecraft
        internal override bool Deadly => false;
        internal override int TileId => 2;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}