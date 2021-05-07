using System;

namespace Hex.Brush
{
    [Serializable]
    public class BrushFeature
    {
        public enum BrushOptionType
        {
            None,
            All,
            Add,
            Minus,
        }

        public BrushOptionType brushOptionType = BrushOptionType.None;
        public int brushRange = 0;
    }
}