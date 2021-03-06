using PDD.DataManagement;

namespace PDD.Tile
{
    public class NormalGroundCornerTopRight : Tile
    {
        protected override string TileTexturePath => "Tile/NormalGroundCornerTR";
        internal override bool IsSolid => true;
        internal override bool CanFall => false;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Safe, "");
        internal override int TileId => 12;
    }
}