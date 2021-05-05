using System;

namespace Hex.Data
{
    [Serializable]
    public class PathFeature: HexFeature
    {
        public enum PathType
        {
            Default,
            Obstacle,
        }
        
        public PathType pathType = PathType.Default;
    }
}