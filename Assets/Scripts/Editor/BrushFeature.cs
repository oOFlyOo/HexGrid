namespace Hex.Editor
{
    public class BrushFeature
    {
        public enum BrushType
        {
            None,
            All,
            Add,
            Minus,
        }

        public BrushType brushType = BrushType.None;
    }
}