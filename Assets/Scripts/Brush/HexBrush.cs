using System;

namespace Hex.Brush
{
    [Serializable]
    public class HexBrush
    {
        public string name;
        public BrushData.BrushType brushType = BrushData.BrushType.None;
        public HexRenderer renderer = new HexRenderer();
    }
}