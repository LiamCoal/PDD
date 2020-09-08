namespace PDD.Tile
{
    public class NormalGroundTurnLeftDown : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundTurnLeftDown";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override bool Deadly => false;
        internal override int TileId => 6;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}