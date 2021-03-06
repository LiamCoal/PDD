using PDD.DataManagement;

namespace PDD.Tile
{
    public class NormalGroundLeftFake : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundRotatedLeft";
        internal override bool IsSolid => false;
        internal override bool CanFall => false; // just like minecraft
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 17;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}