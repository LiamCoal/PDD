using System.Collections.Generic;
using PDD.ConsoleOutput;
// ReSharper disable MemberCanBePrivate.Global

namespace PDD.Tile
{
    public static class Tiles
    {
        public const int
            Portal = -1,
            Air = 0,
            NormalGround = 1,
            NormalGroundLeft = 2,
            NormalGroundDown = 3,
            NormalGroundRight = 4,
            NormalGroundTurnLeft = 5,
            NormalGroundTurnRight = 6,
            NormalGroundTurnLeftDown = 7,
            NormalGroundTurnRightDown = 8,
            NormalGroundCornerBottomRight = 9,
            NormalGroundCornerBottomLeft = 10,
            NormalGroundCornerTopLeft = 11,
            NormalGroundCornerTopRight = 12,
            Spike = 13,
            InvisibleSolid = 14,
            GrippyCeiling = 15,
            NormalGroundFake = 16,
            NormalGroundLeftFake = 17,
            NormalGroundDownFake = 18,
            NormalGroundRightFake = 19,
            Lava = 20,
            Checkpoint = 21;

        public static readonly Dictionary<int, Tile> TileList = new Dictionary<int, Tile>();

        public static void InitializeAll()
        {
            TileList[Air] = new Air();
            TileList[NormalGround] = new NormalGround();
            TileList[NormalGroundLeft] = new NormalGroundLeft();
            TileList[NormalGroundDown] = new NormalGroundDown();
            TileList[NormalGroundRight] = new NormalGroundRight();
            TileList[NormalGroundCornerTopLeft] = new NormalGroundCornerTopLeft();
            TileList[NormalGroundCornerTopRight] = new NormalGroundCornerTopRight();
            TileList[NormalGroundCornerBottomLeft] = new NormalGroundCornerBottomLeft();
            TileList[NormalGroundCornerBottomRight] = new NormalGroundCornerBottomRight();
            TileList[NormalGroundTurnLeft] = new NormalGroundTurnLeft();
            TileList[NormalGroundTurnRight] = new NormalGroundTurnRight();
            TileList[NormalGroundTurnLeftDown] = new NormalGroundTurnLeftDown();
            TileList[NormalGroundTurnRightDown] = new NormalGroundTurnRightDown();
            TileList[Spike] = new Spike();
            TileList[InvisibleSolid] = new InvisibleSolid();
            TileList[GrippyCeiling] = new GrippyCeiling();
            TileList[Portal] = new Portal();
            TileList[NormalGroundFake] = new NormalGroundFake();
            TileList[NormalGroundLeftFake] = new NormalGroundLeftFake();
            TileList[NormalGroundDownFake] = new NormalGroundDownFake();
            TileList[NormalGroundRightFake] = new NormalGroundRightFake();
            TileList[Lava] = new Lava();
            TileList[Checkpoint] = new Checkpoint();
            Logging.Info($"Loaded {TileList.Count} tiles.");
        }
    }
}