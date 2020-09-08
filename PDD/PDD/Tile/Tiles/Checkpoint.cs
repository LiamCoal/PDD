using PDD.DataManagement;

namespace PDD.Tile
{
    public class Checkpoint : AnimatedTile
    {
        protected override string TileTexturePath => "Tile/Checkpoint";
        internal override bool IsSolid => false;
        internal override bool CanFall => true;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 21;
        public override int FrameCount => 2;
        public override int FramesPerSecond => 3;
        public override bool RandomizeFrame => false;
    }
}