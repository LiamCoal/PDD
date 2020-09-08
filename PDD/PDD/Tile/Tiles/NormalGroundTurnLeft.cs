using PDD.DataManagement;

namespace PDD.Tile
{
    public class NormalGroundTurnLeft : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundTurnLeft";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 5;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}