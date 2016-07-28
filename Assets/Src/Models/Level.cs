namespace Assets.Src.Models
{
    using System.Collections.Generic;

    public class Level
    {
        public IList<ElementMap> Elements { get; set; }
    }

    public class ElementMap
    {
        public int Row { get; set; }
        public int Col { get; set; }
        public ElementTypeEnum ElementType { get; set; }
    }
}