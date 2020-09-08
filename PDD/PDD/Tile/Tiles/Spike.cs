using PDD.DataManagement;

namespace PDD.Tile
{
    public class Spike : Tile
    {
        protected override string TileTexturePath => "Tile/Spike";
        internal override bool IsSolid => true;
        internal override bool CanFall => true;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Deadly, "You were impaled...");
        internal override int TileId => 13;
    }
}