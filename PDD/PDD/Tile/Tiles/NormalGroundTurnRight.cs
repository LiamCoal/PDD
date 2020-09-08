namespace PDD.Tile
{
    public class NormalGroundTurnRight : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundTurnRight";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override bool Deadly => false;
        internal override int TileId => 7;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}