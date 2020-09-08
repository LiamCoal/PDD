using PDD.DataManagement;

namespace PDD.Tile
{
    public class NormalGroundTurnLeftDown : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundTurnLeftDown";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 6;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}