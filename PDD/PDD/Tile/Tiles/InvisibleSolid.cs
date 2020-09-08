using PDD.DataManagement;
using PDD.Start;

namespace PDD.Tile
{
    public class InvisibleSolid : Tile
    {
        protected override string TileTexturePath =>
            PddGame.Mode == Mode.LevelEditor ? "Tile/InvisibleSolid" : "Tile/Air";

        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 14;
    }
}