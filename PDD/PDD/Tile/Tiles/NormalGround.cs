using PDD.DataManagement;

namespace PDD.Tile
{
    public class NormalGround : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/NormalGround";
        internal override bool IsSolid => true;
        internal override bool CanFall => false; // just like minecraft
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 1;
        public override int FrameCount => 3;
        public override int FramesPerSecond => 3;
    }
}