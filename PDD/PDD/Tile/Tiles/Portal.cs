using PDD.DataManagement;

namespace PDD.Tile
{
    public class Portal : Tile
    {
        protected override string TileTexturePath => "Tile/Portal";
        internal override bool IsSolid => false;
        internal override bool CanFall => false;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => -1;
    }
}