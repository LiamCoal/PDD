using PDD.DataManagement;

namespace PDD.Tile
{
    public class NormalGroundTurnRightDown : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGroundTurnRightDown";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 8;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}