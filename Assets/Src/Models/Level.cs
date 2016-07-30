using System;

namespace Assets.Src.Models
{
    [Serializable]
    public class Level
    {
        public Tile[] TileMap { get; set; }
    }

    [Serializable]
    public class Tile
    {
        public int Row { get; set; }

        public int Col { get; set; }

        public TileTypeEnum TileType { get; set; }
    }
}