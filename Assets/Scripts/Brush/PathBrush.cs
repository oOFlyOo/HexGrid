using System;
using Hex.Data;

namespace Hex.Brush
{
    [Serializable]
    public class PathBrush: HexBrush
    {
        public PathFeature path;

        public PathBrush()
        {
            brushType = BrushData.BrushType.Path;
            renderer.minSize = 0;
        }
    }
}