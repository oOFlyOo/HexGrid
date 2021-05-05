using System;

namespace Hex.Brush
{
    [Serializable]
    public class HexBrush
    {
        public string name;
        public HexRenderer renderer = new HexRenderer();
    }
}