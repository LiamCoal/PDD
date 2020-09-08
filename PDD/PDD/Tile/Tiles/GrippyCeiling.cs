namespace PDD.Tile
{
    public class GrippyCeiling : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/GrippyCeiling";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override bool Deadly => false;
        internal override int TileId => 15;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}