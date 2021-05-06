using System.Collections.Generic;
using System.Linq;
using Hex.Data;
using UnityEngine;

namespace Hex.Brush
{
    public class BrushData : ScriptableObject
    {
        public enum BrushType
        {
            None,
            Path,
        }
        
        public List<PathBrush> pathBrushes;

        public PathBrush GetPathBrushByPathType(PathFeature.PathType type)
        {
            var brush = pathBrushes.FirstOrDefault(pathBrush => pathBrush.path.pathType == type);
            return brush;
        }

        public bool HasPathBrushes()
        {
            return pathBrushes != null && pathBrushes.Count > 0;
        }
    }
}