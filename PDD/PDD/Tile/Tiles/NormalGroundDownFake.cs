using PDD.DataManagement;

namespace PDD.Tile
{
    public class NormalGroundDownFake : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundRotatedDown";
        internal override bool IsSolid => false;
        internal override bool CanFall => false; // just like minecraft
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 18;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}