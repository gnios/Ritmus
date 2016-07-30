using System;

namespace Assets.Src.Models
{
    [Serializable]
    public class Level
    {
        public ElementMap[] Elements { get; set; }
    }

    [Serializable]
    public class ElementMap
    {
        public int Row { get; set; }

        public int Col { get; set; }

        public TileTypeEnum TileType { get; set; }
    }
}