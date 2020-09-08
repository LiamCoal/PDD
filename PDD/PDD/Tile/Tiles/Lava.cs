using PDD.DataManagement;

namespace PDD.Tile
{
    public class Lava : Tile
    {
        protected override string TileTexturePath => "Tile/Lava";
        internal override bool IsSolid => false;
        internal override bool CanFall => false;
        internal override DeadlinessInfo Deadliness => new DeadlinessInfo(DeadlinessInfo.Deadly.Deadly, "You were burned alive...");
        internal override int TileId => 20;
    }
}