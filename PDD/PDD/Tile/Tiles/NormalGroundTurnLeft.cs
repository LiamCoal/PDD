namespace PDD.Tile
{
    public class NormalGroundTurnLeft : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundTurnLeft";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override bool Deadly => false;
        internal override int TileId => 5;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}